using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking.API.Data;
using MyCoreBanking.API.Data.Entities;
using MyCoreBanking.Models;

namespace FreedomHub.API.Services.Auth;

public static class TransacoesGet
{
    [FunctionName(nameof(TransacoesGet))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "transacoes")] HttpRequest httpRequest,
        ILogger logger)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            IQueryable<TransacaoEntity> query = context.Transacoes
                .AsNoTrackingWithIdentityResolution()
                .Include(t => t.MeioDePagamento)
                .Where(t => t.UsuarioId == userId)
                .OrderByDescending(t => t.CriadoEm);

            if (httpRequest.Query.TryGetValue("meioDePagamentoId", out var meioDePagamentoId)
                && Guid.TryParse(meioDePagamentoId, out var meioDePagamentoIdGuid))
            {
                query = query.Where(t => t.MeioDePagamentoId == meioDePagamentoIdGuid);
            }

            var transacoes = await query
                .Select(t => t.ToModel())
                .ToListAsync();

            var result = new List<TransacaoModel>(transacoes).AsReadOnly();

            return httpRequest.CreateResult(result);
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}