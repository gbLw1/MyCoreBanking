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
using MyCoreBanking.Models;

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
                .Include(t => t.MeioDePagamento)
                .Where(t => t.UsuarioId == userId)
                .Where(t => t.Id == transacaoId)
                .OrderByDescending(t => t.CriadoEm);

            if (httpRequest.Query.TryGetValue("meioDePagamentoId", out var meioDePagamentoId)
                && Guid.TryParse(meioDePagamentoId, out var meioDePagamentoIdGuid))
            {
                query = query.Where(t => t.MeioDePagamentoId == meioDePagamentoIdGuid);
            }

            var transacaoModel = await query
                .Select(t => t.ToModel())
                .FirstOrDefaultAsync();

            if (transacaoModel is null)
                throw new NotFoundException(message: "Transação não encontrada", paramName: nameof(transacaoId));

            return httpRequest.CreateResult(transacaoModel);
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}