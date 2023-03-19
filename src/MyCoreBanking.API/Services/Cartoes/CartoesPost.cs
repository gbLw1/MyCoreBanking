using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking;
using MyCoreBanking.API.Data;
using MyCoreBanking.API.Data.Entities;
using MyCoreBanking.Args;

namespace FreedomHub.API.Services.Auth;

public static class CartoesPost
{
    [FunctionName(nameof(CartoesPost))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "cartoes")] HttpRequest httpRequest,
        ILogger logger)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<CartoesPostArgs>();

            new CartoesPostArgs.Validator().ValidateAndThrow(args);

            var cartaoEntity = new CartaoDeCreditoEntity
            {
                NumerosFinais = args.NumerosFinais,
                Banco = args.Banco is null ? null : args.Banco,
                Bandeira = args.Bandeira,
                MeioDePagamento = new MeioDePagamentoEntity
                {
                    Apelido = $"{args.Bandeira}_{args.NumerosFinais}",
                    Observacao = args.Observacao,
                    Tipo = MeioDePagamentoTipo.CartaoDeCredito,
                    UsuarioId = userId,
                },
            };

            context.CartoesDeCredito.Add(cartaoEntity);

            await context.SaveChangesAsync();

            return httpRequest.CreateResult(new { cartaoEntity.Id });
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}