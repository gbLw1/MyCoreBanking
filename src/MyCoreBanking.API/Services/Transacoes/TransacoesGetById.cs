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

namespace FreedomHub.API.Services.Auth;

public static class TransacoesGetById
{
    [FunctionName(nameof(TransacoesGetById))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "transacoes/{transacaoId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid transacaoId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            IQueryable<TransacaoEntity> query = context.Transacoes
                .AsNoTrackingWithIdentityResolution()
                .Where(t => t.UsuarioId == userId)
                .Where(t => t.Id == transacaoId)
                .OrderByDescending(t => t.DataDaTransacao);

            var transacao = await query
                .Select(t => t.ToModel())
                .FirstOrDefaultAsync();

            if (transacao is null)
                throw new NotFoundException(message: "Transação não encontrada", paramName: nameof(transacaoId));

            return httpRequest.CreateResult(transacao);
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}