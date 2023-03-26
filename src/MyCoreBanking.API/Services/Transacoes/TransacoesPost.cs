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

            switch (args.TipoDeTransacao)
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
                            TipoDeOperacao = args.TipoDeOperacao,
                            MeioDePagamento = args.MeioDePagamento,
                            Categoria = args.Categoria,
                            DataPagamento = args.DataPagamento,
                        };

                        context.Transacoes.Add(transacaoEntity);
                        await context.SaveChangesAsync();

                        if (args.DataPagamento.HasValue)
                        {
                            if (args.TipoDeOperacao == OperacaoTipo.Despesa)
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
                    for (int i = 0; i < args.NumeroParcelas!.Value; i++)
                    {
                        transacaoEntity = new()
                        {
                            UsuarioId = userId,
                            ContaId = args.ContaId,
                            Descricao = $"{args.Descricao} - ({i + 1}/{args.NumeroParcelas.Value})",
                            Observacao = args.Observacao,
                            DataPagamento = null,
                            Valor = args.ValorParcela!.Value * args.NumeroParcelas.Value,
                            TipoDeOperacao = args.TipoDeOperacao,
                            TipoDeTransacao = TransacaoTipo.Parcelada,
                            MeioDePagamento = args.MeioDePagamento,
                            Categoria = args.Categoria,
                            InicioParcelamento = args.InicioParcelamento,
                            DataVencimento = args.DataVencimento,
                            NumeroParcelas = args.NumeroParcelas,
                            ValorParcela = args.ValorParcela,
                        };

                        context.Transacoes.Add(transacaoEntity);

                        await context.SaveChangesAsync();
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