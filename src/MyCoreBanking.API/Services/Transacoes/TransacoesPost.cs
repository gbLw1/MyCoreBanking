using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            var transacaoEntity = new TransacaoEntity
            {
                Descricao = args.Descricao,
                Observacao = args.Observacao,
                Valor = args.Valor,
                DataPagamento = args.DataPagamento,
                UsuarioId = userId,
                MeioDePagamentoId = args.MeioDePagamentoId,
            };

            context.Transacoes.Add(transacaoEntity);

            await context.SaveChangesAsync();

            return httpRequest.CreateResult(new { transacaoEntity.Id });
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}