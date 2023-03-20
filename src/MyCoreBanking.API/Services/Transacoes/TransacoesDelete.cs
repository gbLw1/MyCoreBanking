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

public static class TransacoesDelete
{
    [FunctionName(nameof(TransacoesDelete))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "transacoes/{transacaoId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid transacaoId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var transacao = await context.Transacoes
                .Include(t => t.MeioDePagamento)
                .Where(t => t.UsuarioId == userId)
                .FirstOrDefaultAsync(t => t.Id == transacaoId);

            // tenta obter a transacao
            if (transacao is null)
                throw new NotFoundException(message: "Transação não encontrada", paramName: nameof(transacaoId));

            context.Transacoes.Remove(transacao);
            await context.SaveChangesAsync();

            return httpRequest.CreateResult();
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}