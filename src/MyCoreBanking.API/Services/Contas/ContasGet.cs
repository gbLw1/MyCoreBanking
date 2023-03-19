using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking.API.Data;
using MyCoreBanking.Models;

namespace FreedomHub.API.Services.Auth;

public static class ContasGet
{
    [FunctionName(nameof(ContasGet))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "contas")] HttpRequest httpRequest,
        ILogger logger)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var contas = await context.ContasCorrente
                .Include(c => c.MeioDePagamento)
                .Where(c => c.MeioDePagamento.UsuarioId == userId)
                .OrderByDescending(c => c.MeioDePagamento.CriadoEm)
                .Select(c => c.ToModel())
                .ToListAsync();

            var result = new List<ContaCorrenteModel>(contas).AsReadOnly();

            return httpRequest.CreateResult(result);
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}