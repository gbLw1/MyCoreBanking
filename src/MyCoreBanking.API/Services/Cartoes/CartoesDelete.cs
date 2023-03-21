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

public static class CartoesDelete
{
    [FunctionName(nameof(CartoesDelete))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "cartoes/{cartaoId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid cartaoId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var cartaoEntity = await context.CartoesDeCredito
                .Include(c => c.MeioDePagamento)
                .Where(c => c.MeioDePagamento.UsuarioId == userId)
                .FirstOrDefaultAsync(c => c.Id == cartaoId);

            if (cartaoEntity is null)
                throw new NotFoundException(message: "Cartão não encontrado", paramName: nameof(cartaoId));

            // Verificar se o cartão está vinculado a alguma transacao
            if (await context.Transacoes.AnyAsync(t => t.MeioDePagamentoId == cartaoId))
                throw new InvalidOperationException(message: "Não é possível excluir um cartão que está vinculado a uma transação");

            context.CartoesDeCredito.Remove(cartaoEntity);
            await context.SaveChangesAsync();

            return httpRequest.CreateResult();
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}