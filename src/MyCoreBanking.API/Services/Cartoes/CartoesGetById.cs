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

public static class CartoesGetById
{
    [FunctionName(nameof(CartoesGetById))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "cartoes/{cartaoId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid cartaoId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var cartao = await context.CartoesDeCredito
                .Include(c => c.MeioDePagamento)
                .Where(c => c.MeioDePagamento.UsuarioId == userId)
                .Where(c => c.Id == cartaoId)
                .Select(c => c.ToModel())
                .FirstOrDefaultAsync();

            if (cartao is null)
                throw new NotFoundException(message: "Cartão não encontrado", paramName: nameof(cartaoId));

            return httpRequest.CreateResult(cartao);
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}