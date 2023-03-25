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
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "transacoes/{contaId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid contaId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var conta = await context.Contas
                .FirstOrDefaultAsync(x => x.Id == contaId);

            if (conta is null)
                throw new NotFoundException(message: "Conta não encontrada", paramName: nameof(contaId));

            var args = await httpRequest.ReadJsonBodyAsAsync<TransacoesPostArgs>();

            new TransacoesPostArgs.Validator().ValidateAndThrow(args);

            // TODO: Se for transação recorrente, DataVigenciaInicio e DataVigenciaFim são obrigatórias

            // TODO: Se for transação parcelada, DiaVencimento, NumeroParcelas e ValorParcela são obrigatórias

            var transacaoEntity = new TransacaoEntity
            {
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