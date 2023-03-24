using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking.API.Data;

namespace MyCoreBanking.API.Services.Usuarios;

public class ContasDELETE
{
    [FunctionName(nameof(ContasDELETE))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "contas/{contaId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid contaId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            // Regra para não permitir que o usuário exclua a última conta
            var contas = await context.Contas
                .Where(c => c.UsuarioId == userId)
                .CountAsync();

            if (contas == 1)
                throw new InvalidOperationException(message: "Falha ao realizar a operação! Mínimo de 01 conta cadastrada.");

            var conta = await context.Contas
                .Where(c => c.UsuarioId == userId)
                .Where(c => c.Id == contaId)
                .FirstOrDefaultAsync();

            if (conta is null)
                throw new NotFoundException(message: "Conta não encontrada", paramName: nameof(conta));

            context.Contas.Remove(conta);
            await context.SaveChangesAsync();

            return httpRequest.CreateResult();
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}
