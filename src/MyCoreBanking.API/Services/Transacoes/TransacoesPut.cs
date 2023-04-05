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

                        // Se a transação já estava efetivada, altera o saldo da conta com o novo valor
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

                        // Se a transação NÃO estava efetivada e está efetivando, atualiza o saldo da conta e efetiva a transação
                        if (transacaoEntity.DataEfetivacao is null && args.DataEfetivacao.HasValue)
                        {
                            if (transacaoEntity.TipoOperacao == OperacaoTipo.Receita)
                            {
                                transacaoEntity.Conta!.Saldo += args.Valor;
                            }
                            else
                            {
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

                                // Se a transação já estava efetivada, altera o saldo da conta com o novo valor
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

                                // Se a transação NÃO estava efetivada e está efetivando, atualiza o saldo da conta e efetiva a transação
                                if (transacaoEntity.DataEfetivacao is null && args.DataEfetivacao.HasValue)
                                {
                                    if (transacaoEntity.TipoOperacao == OperacaoTipo.Receita)
                                    {
                                        transacaoEntity.Conta!.Saldo += args.Valor;
                                    }
                                    else
                                    {
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
                                transacaoEntity.DataTransacao = new DateTime(transacaoEntity.DataTransacao.Year, transacaoEntity.DataTransacao.Month, args.DataTransacao.Day);

                                transacaoEntity.UltimaAtualizacaoEm = DateTime.Now;

                                context.Transacoes.Update(transacaoEntity);
                                await context.SaveChangesAsync();


                                // ↓ Atualiza o valor total do parcelamento nas outras parcelas ↓
                                var parcelas = await context.Transacoes
                                    .Where(t => t.UsuarioId == userId)
                                    .Where(t => t.ReferenciaParcelaId == transacaoEntity.ReferenciaParcelaId)
                                    .ToListAsync();

                                foreach (var transacao in parcelas)
                                {
                                    transacao.Valor = transacaoEntity.Valor;
                                    transacao.UltimaAtualizacaoEm = DateTime.Now;
                                }

                                context.Transacoes.UpdateRange(parcelas);
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
                                    .Include(t => t.Conta)
                                    .Where(t => t.UsuarioId == userId)
                                    .Where(t => t.ReferenciaParcelaId == transacaoEntity.ReferenciaParcelaId)
                                    .Where(t => t.DataEfetivacao == null);

                                var transacoesPendentes = await query.ToListAsync();

                                // Verifica se a transação selecionada pelo Id já está na lista de transações pendentes e inclui se não estiver
                                if (!transacoesPendentes.Any(t => t.Id == transacaoEntity.Id))
                                {
                                    transacoesPendentes.Add(transacaoEntity);
                                }

                                foreach (var transacao in transacoesPendentes)
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

                                    // Se a transação já estava efetivada, altera o saldo da conta com o novo valor
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

                                    // Se a transação NÃO estava efetivada e está efetivando todas as parcelas pendentes, atualiza o saldo da conta e efetiva a transação
                                    if (transacao.DataEfetivacao is null && args.DataEfetivacao.HasValue)
                                    {
                                        if (transacao.TipoOperacao == OperacaoTipo.Receita)
                                        {
                                            transacao.Conta!.Saldo += args.Valor;
                                        }
                                        else
                                        {
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
                                    transacao.DataTransacao = new DateTime(transacao.DataTransacao.Year, transacao.DataTransacao.Month, args.DataTransacao.Day);

                                    transacao.UltimaAtualizacaoEm = DateTime.Now;

                                    context.Transacoes.Update(transacao);
                                    await context.SaveChangesAsync();
                                }

                                // ↓ Atualiza o valor total do parcelamento em TODAS as outras parcelas ↓
                                var parcelas = await context.Transacoes
                                    .Where(t => t.UsuarioId == userId)
                                    .Where(t => t.ReferenciaParcelaId == transacaoEntity.ReferenciaParcelaId)
                                    .ToListAsync();

                                foreach (var parcela in parcelas)
                                {
                                    parcela.Valor = parcela.ValorParcela!.Value * parcela.NumeroParcelas!.Value;
                                    parcela.UltimaAtualizacaoEm = DateTime.Now;
                                }

                                context.Transacoes.UpdateRange(parcelas);
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

                                    // Se a transação já estava efetivada, altera o saldo da conta com o novo valor
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

                                    // Se a transação NÃO estava efetivada e está efetivando todas as parcelas pendentes, atualiza o saldo da conta e efetiva a transação
                                    if (transacao.DataEfetivacao is null && args.DataEfetivacao.HasValue)
                                    {
                                        if (transacao.TipoOperacao == OperacaoTipo.Receita)
                                        {
                                            transacao.Conta!.Saldo += args.Valor;
                                        }
                                        else
                                        {
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
                                    transacao.DataTransacao = new DateTime(transacao.DataTransacao.Year, transacao.DataTransacao.Month, args.DataTransacao.Day);

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