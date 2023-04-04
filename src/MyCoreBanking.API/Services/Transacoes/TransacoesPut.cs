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
using MyCoreBanking.Args;

namespace FreedomHub.API.Services.Auth;

public static class TransacoesPut
{
    [FunctionName(nameof(TransacoesPut))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "transacoes/{transacaoId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid transacaoId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<TransacoesPutArgs>();

            new TransacoesPutArgs.Validator().ValidateAndThrow(args);

            // Tenta obter a transação
            var transacaoEntity = await context.Transacoes
                .Include(t => t.Conta) // include na conta para atualizar o saldo
                .Where(t => t.UsuarioId == userId)
                .FirstOrDefaultAsync(t => t.Id == transacaoId);

            if (transacaoEntity is null)
                throw new NotFoundException(message: "Transação não encontrada", paramName: nameof(transacaoId));

            var queryParameters = httpRequest.GetQueryParameterDictionary();
            var dbTransaction = await context.Database.BeginTransactionAsync();

            switch (transacaoEntity.TipoTransacao)
            {
                case TransacaoTipo.Unica:
                    try
                    {
                        // Se a transação já estava efetivada e está desfazendo, estorna o saldo da conta
                        if (transacaoEntity.DataEfetivacao.HasValue && args.DataEfetivacao is null)
                        {
                            if (transacaoEntity.TipoOperacao == OperacaoTipo.Receita)
                            {
                                transacaoEntity.Conta!.Saldo -= transacaoEntity.Valor;
                            }
                            else
                            {
                                transacaoEntity.Conta!.Saldo += transacaoEntity.Valor;
                            }
                        }

                        // Atualizar o saldo da conta com o novo valor se já estava efetivada
                        if (transacaoEntity.DataEfetivacao.HasValue && args.DataEfetivacao.HasValue)
                        {
                            if (transacaoEntity.TipoOperacao == OperacaoTipo.Receita)
                            {
                                transacaoEntity.Conta!.Saldo -= transacaoEntity.Valor;
                                transacaoEntity.Conta!.Saldo += args.Valor;
                            }
                            else
                            {
                                transacaoEntity.Conta!.Saldo += transacaoEntity.Valor;
                                transacaoEntity.Conta!.Saldo -= args.Valor;
                            }
                        }

                        transacaoEntity.Descricao = args.Descricao;
                        transacaoEntity.Observacao = args.Observacao;
                        transacaoEntity.Valor = args.Valor;
                        transacaoEntity.DataEfetivacao = args.DataEfetivacao;
                        transacaoEntity.DataTransacao = args.DataTransacao;
                        transacaoEntity.MeioPagamento = args.MeioPagamento;
                        transacaoEntity.Categoria = args.Categoria;

                        transacaoEntity.UltimaAtualizacaoEm = DateTime.Now;

                        context.Transacoes.Update(transacaoEntity);
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
                    if (!queryParameters.TryGetValue("tipoUpdate", out var tipoUpdate))
                    {
                        throw new InvalidOperationException(message: "O parâmetro 'tipoUpdate' é obrigatório para transações parceladas");
                    }

                    switch (tipoUpdate.ToUpper())
                    {
                        case "UNICO":
                            try
                            {
                                // Se a transação já estava efetivada e está desfazendo, estorna o saldo da conta
                                if (transacaoEntity.DataEfetivacao.HasValue && args.DataEfetivacao is null)
                                {
                                    if (transacaoEntity.TipoOperacao == OperacaoTipo.Receita)
                                    {
                                        transacaoEntity.Conta!.Saldo -= transacaoEntity.ValorParcela!.Value;
                                    }
                                    else
                                    {
                                        transacaoEntity.Conta!.Saldo += transacaoEntity.ValorParcela!.Value;
                                    }
                                }

                                // Atualizar o saldo da conta com o novo valor se já estava efetivada
                                if (transacaoEntity.DataEfetivacao.HasValue && args.DataEfetivacao.HasValue)
                                {
                                    if (transacaoEntity.TipoOperacao == OperacaoTipo.Receita)
                                    {
                                        transacaoEntity.Conta!.Saldo -= transacaoEntity.ValorParcela!.Value;
                                        transacaoEntity.Conta!.Saldo += args.Valor;
                                    }
                                    else
                                    {
                                        transacaoEntity.Conta!.Saldo += transacaoEntity.ValorParcela!.Value;
                                        transacaoEntity.Conta!.Saldo -= args.Valor;
                                    }
                                }

                                // Reajuste valor da transação
                                transacaoEntity.Valor -= transacaoEntity.ValorParcela!.Value;
                                transacaoEntity.Valor += args.Valor;

                                // Atualizar os dados da transação
                                transacaoEntity.Descricao = args.Descricao;
                                transacaoEntity.Observacao = args.Observacao;
                                transacaoEntity.ValorParcela = args.Valor;
                                transacaoEntity.DataEfetivacao = args.DataEfetivacao;

                                // Atualizar o valor total do parcelamento nas outras parcelas
                                var parcelas = await context.Transacoes
                                    .Where(t => t.UsuarioId == userId)
                                    .Where(t => t.ReferenciaParcelaId == transacaoEntity.ReferenciaParcelaId)
                                    .ToListAsync();

                                foreach (var transacao in parcelas)
                                {
                                    transacao.Valor = transacaoEntity.Valor;
                                    transacao.UltimaAtualizacaoEm = DateTime.Now;
                                }

                                context.Transacoes.Update(transacaoEntity);
                                await context.SaveChangesAsync();
                                await dbTransaction.CommitAsync();
                            }
                            catch (Exception)
                            {
                                await dbTransaction.RollbackAsync();
                                throw;
                            }
                            break;

                        case "PAGAMENTO-PENDENTE":
                            try
                            {
                                var query = context.Transacoes
                                    .AsNoTrackingWithIdentityResolution()
                                    .Include(t => t.Conta)
                                    .Where(t => t.UsuarioId == userId);

                                query = query.Where(t => t.ReferenciaParcelaId == transacaoEntity.ReferenciaParcelaId);
                                query = query.Where(t => t.DataEfetivacao == null);

                                var transacoesPendentes = await query.ToListAsync();

                                foreach (var transacao in transacoesPendentes)
                                {
                                    // Atualiza o saldo da conta se está efetivando todas as parcelas pendentes
                                    if (args.DataEfetivacao.HasValue)
                                    {
                                        if (transacao.TipoOperacao == OperacaoTipo.Receita)
                                        {
                                            transacao.Conta!.Saldo -= transacao.ValorParcela!.Value;
                                            transacao.Conta!.Saldo += args.Valor;
                                        }
                                        else
                                        {
                                            transacao.Conta!.Saldo += transacao.ValorParcela!.Value;
                                            transacao.Conta!.Saldo -= args.Valor;
                                        }
                                    }

                                    // Reajuste valor da transação
                                    transacao.Valor -= transacaoEntity.ValorParcela!.Value;
                                    transacao.Valor += args.Valor;

                                    // Atualizar os dados da transação
                                    transacao.Descricao = args.Descricao;
                                    transacao.Observacao = args.Observacao;
                                    transacao.ValorParcela = args.Valor;
                                    transacao.DataEfetivacao = args.DataEfetivacao;

                                    transacao.UltimaAtualizacaoEm = DateTime.Now;

                                    context.Transacoes.Update(transacao);
                                    await context.SaveChangesAsync();
                                }

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
                                var query = context.Transacoes
                                    .Include(t => t.Conta)
                                    .Where(t => t.UsuarioId == userId);

                                query = query.Where(t => t.ReferenciaParcelaId == transacaoEntity.ReferenciaParcelaId);

                                var transacoes = await query.ToListAsync();

                                foreach (var transacao in transacoes)
                                {
                                    // Se a transação já estava efetivada e está desfazendo, estorna o saldo da conta
                                    if (transacao.DataEfetivacao.HasValue && args.DataEfetivacao is null)
                                    {
                                        if (transacao.TipoOperacao == OperacaoTipo.Receita)
                                        {
                                            transacao.Conta!.Saldo -= transacao.ValorParcela!.Value;
                                        }
                                        else
                                        {
                                            transacao.Conta!.Saldo += transacao.ValorParcela!.Value;
                                        }
                                    }

                                    // Atualizar o saldo da conta com o novo valor se já estava efetivada
                                    if (transacao.DataEfetivacao.HasValue && args.DataEfetivacao.HasValue)
                                    {
                                        if (transacao.TipoOperacao == OperacaoTipo.Receita)
                                        {
                                            transacao.Conta!.Saldo -= transacao.ValorParcela!.Value;
                                            transacao.Conta!.Saldo += args.Valor;
                                        }
                                        else
                                        {
                                            transacao.Conta!.Saldo += transacao.ValorParcela!.Value;
                                            transacao.Conta!.Saldo -= args.Valor;
                                        }
                                    }

                                    // Reajuste valor da transação
                                    transacao.Valor -= transacaoEntity.ValorParcela!.Value;
                                    transacao.Valor += args.Valor;

                                    // Atualizar os dados da transação
                                    transacao.Descricao = args.Descricao;
                                    transacao.Observacao = args.Observacao;
                                    transacao.ValorParcela = args.Valor;
                                    transacao.DataEfetivacao = args.DataEfetivacao;

                                    transacao.UltimaAtualizacaoEm = DateTime.Now;

                                    context.Transacoes.Update(transacao);
                                    await context.SaveChangesAsync();
                                }

                                await dbTransaction.CommitAsync();
                            }
                            catch (Exception)
                            {
                                await dbTransaction.RollbackAsync();
                                throw;
                            }
                            break;

                        default: throw new IndexOutOfRangeException(message: "Tipo de update inválido. valores aceitos: UNICO, PAGAMENTO-PENDENTE, TODOS");
                    }
                    break;

                default: throw new IndexOutOfRangeException(message: "Tipo de transação desconhecida.");
            }

            context.Transacoes.Update(transacaoEntity);
            await context.SaveChangesAsync();

            return httpRequest.CreateResult(new { transacaoEntity.Id });
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}