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

public static class CartoesPut
{
    [FunctionName(nameof(CartoesPut))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "cartoes/{cartaoId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid cartaoId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<CartoesPutArgs>();

            new CartoesPutArgs.Validator().ValidateAndThrow(args);

            var cartaoEntity = await context.CartoesDeCredito
                .Include(c => c.MeioDePagamento)
                .FirstOrDefaultAsync(c => c.Id == cartaoId);

            if (cartaoEntity is null)
                throw new NotFoundException(message: "Cartão não encontrado", paramName: nameof(cartaoId));

            cartaoEntity.NumerosFinais = args.NumerosFinais;
            cartaoEntity.Banco = args.Banco is null ? null : args.Banco;
            cartaoEntity.Bandeira = args.Bandeira;
            cartaoEntity.MeioDePagamento.Apelido = $"{args.Bandeira} - **** **** **** {args.NumerosFinais}";
            cartaoEntity.MeioDePagamento.Observacao = args.Observacao;

            context.CartoesDeCredito.Update(cartaoEntity);
            await context.SaveChangesAsync();

            return httpRequest.CreateResult(new { cartaoEntity.Id });
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}