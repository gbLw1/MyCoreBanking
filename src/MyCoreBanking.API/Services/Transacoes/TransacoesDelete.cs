using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking;
using MyCoreBanking.API.Data;

namespace FreedomHub.API.Services.Auth;

public static class TransacoesDelete
{
    [FunctionName(nameof(TransacoesDelete))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "transacoes/{transacaoId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid transacaoId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            // Verifica se a transação pertence ao usuário e é recorrente
            var query = context.Transacoes
                .Include(t => t.Conta)
                .Where(t => t.UsuarioId == userId);

            // tenta obter a transacao pelo id informado
            var transacao = await query
                .Where(t => t.Id == transacaoId)
                .FirstOrDefaultAsync();

            if (transacao is null)
                throw new NotFoundException(message: "Transação não encontrada", paramName: nameof(transacaoId));

            var queryParameters = httpRequest.GetQueryParameterDictionary();
            var dbTransaction = await context.Database.BeginTransactionAsync();

            switch (transacao.TipoTransacao)
            {
                case TransacaoTipo.Unica:
                    try
                    {
                        if (transacao.DataEfetivacao.HasValue)
                        {
                            if (transacao.TipoOperacao == OperacaoTipo.Despesa)
                            {
                                transacao.Conta!.Saldo += transacao.Valor;
                            }
                            else
                            {
                                transacao.Conta!.Saldo -= transacao.Valor;
                            }
                        }

                        context.Transacoes.Remove(transacao);
                        await context.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await dbTransaction.RollbackAsync();
                        throw;
                    }
                    break;

                case TransacaoTipo.Parcelada:
                    if (!queryParameters.TryGetValue("tipoDelete", out var tipoDelete))
                    {
                        throw new InvalidOperationException(message: "O parâmetro 'tipoDelete' é obrigatório para transações parceladas");
                    }

                    switch (tipoDelete.ToUpper())
                    {
                        case "UNICO":
                            try
                            {
                                if (transacao.DataEfetivacao.HasValue)
                                {
                                    if (transacao.TipoOperacao == OperacaoTipo.Despesa)
                                    {
                                        transacao.Conta!.Saldo += transacao.ValorParcela!.Value;
                                    }
                                    else
                                    {
                                        transacao.Conta!.Saldo -= transacao.ValorParcela!.Value;
                                    }
                                }

                                context.Transacoes.Remove(transacao);
                                await context.SaveChangesAsync();
                                await dbTransaction.CommitAsync();
                            }
                            catch (Exception)
                            {
                                await dbTransaction.RollbackAsync();
                                throw;
                            }
                            break;

                        case "TODOS":
                            try
                            {
                                query = query.Where(t => t.ReferenciaParcelaId == transacao.ReferenciaParcelaId);

                                var todasTransacoesParceladas = await query.ToListAsync();

                                foreach (var transacaoParcelada in todasTransacoesParceladas)
                                {
                                    if (transacaoParcelada.DataEfetivacao.HasValue)
                                    {
                                        if (transacaoParcelada.TipoOperacao == OperacaoTipo.Despesa)
                                        {
                                            transacaoParcelada.Conta!.Saldo += transacaoParcelada.ValorParcela!.Value;
                                        }
                                        else
                                        {
                                            transacaoParcelada.Conta!.Saldo -= transacaoParcelada.ValorParcela!.Value;
                                        }
                                    }
                                }

                                context.Transacoes.RemoveRange(todasTransacoesParceladas);
                                await context.SaveChangesAsync();
                                await dbTransaction.CommitAsync();

                                // TODO: Reordenar as parcelas para que não haja gaps
                            }
                            catch (Exception)
                            {
                                await dbTransaction.RollbackAsync();
                                throw;
                            }
                            break;

                        default: throw new IndexOutOfRangeException(message: "Tipo de delete inválido. valores aceitos: UNICO, TODOS");
                    }
                    break;

                default: throw new IndexOutOfRangeException(message: "Tipo de transação desconhecida.");
            }

            return httpRequest.CreateResult();
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}