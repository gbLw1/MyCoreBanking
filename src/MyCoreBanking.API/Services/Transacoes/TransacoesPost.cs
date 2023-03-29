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
using MyCoreBanking.API.Data.Entities;
using MyCoreBanking.Args;

namespace FreedomHub.API.Services.Auth;

public static class TransacoesPost
{
    [FunctionName(nameof(TransacoesPost))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "transacoes")] HttpRequest httpRequest,
        ILogger logger)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<TransacoesPostArgs>();

            new TransacoesPostArgs.Validator().ValidateAndThrow(args);

            var conta = await context.Contas
                .Where(c => c.UsuarioId == userId)
                .Where(c => c.Id == args.ContaId)
                .FirstOrDefaultAsync();

            if (conta is null)
                throw new NotFoundException(message: "Conta não encontrada", paramName: nameof(args.ContaId));

            TransacaoEntity transacaoEntity = new();
            using var dbTransaction = await context.Database.BeginTransactionAsync();

            switch (args.TipoTransacao)
            {
                case TransacaoTipo.Unica:
                    try
                    {
                        transacaoEntity = new()
                        {
                            UsuarioId = userId,
                            ContaId = args.ContaId,
                            Descricao = args.Descricao,
                            Observacao = args.Observacao,
                            Valor = args.Valor!.Value,
                            TipoOperacao = args.TipoOperacao,
                            MeioPagamento = args.MeioPagamento,
                            Categoria = args.Categoria,
                            DataEfetivacao = args.DataEfetivacao,
                            DataTransacao = args.DataTransacao!.Value,
                        };

                        await context.AddAsync(transacaoEntity);
                        await context.SaveChangesAsync();

                        if (args.DataEfetivacao.HasValue)
                        {
                            if (args.TipoOperacao == OperacaoTipo.Despesa)
                            {
                                conta.Saldo -= args.Valor!.Value;
                            }
                            else
                            {
                                conta.Saldo += args.Valor!.Value;
                            }

                            conta.UltimaAtualizacaoEm = DateTime.Now;

                            context.Contas.Update(conta);
                            await context.SaveChangesAsync();
                        }

                        dbTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbTransaction.Rollback();
                        throw;
                    }
                    break;

                case TransacaoTipo.Parcelada:
                    try
                    {
                        Guid parcelaId = Guid.NewGuid();

                        for (int i = 0; i < args.NumeroParcelas!.Value; i++)
                        {
                            transacaoEntity = new()
                            {
                                UsuarioId = userId,
                                ContaId = args.ContaId,
                                ReferenciaParcelaId = parcelaId,
                                Descricao = args.Descricao,
                                ParcelaAtual = i + 1,
                                Observacao = args.Observacao,
                                DataEfetivacao = null,
                                DataTransacao = args.InicioParcelamento ?? DateTime.Now,
                                Valor = args.ValorParcela!.Value * args.NumeroParcelas.Value,
                                TipoOperacao = args.TipoOperacao,
                                TipoTransacao = TransacaoTipo.Parcelada,
                                MeioPagamento = args.MeioPagamento,
                                Categoria = args.Categoria,
                                DataVencimento = args.DataVencimento!.Value.AddMonths(i),
                                NumeroParcelas = args.NumeroParcelas,
                                ValorParcela = args.ValorParcela,
                            };

                            context.Transacoes.Add(transacaoEntity);

                            await context.SaveChangesAsync();
                        }

                        dbTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbTransaction.Rollback();
                        throw;
                    }
                    break;

                default: throw new IndexOutOfRangeException(message: "Tipo de transação não suportado");
            }

            return httpRequest.CreateResult();
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}