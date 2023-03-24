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
using MyCoreBanking.Args;

namespace MyCoreBanking.API.Services.Usuarios;

public class ContasPOST
{
    [FunctionName(nameof(ContasPOST))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "contas")] HttpRequest httpRequest,
        ILogger logger)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<ContasPostArgs>();

            new ContasPostArgs.Validator().ValidateAndThrow(args);

            // Regra para o usuario não poder cadastrar mais de 5 contas
            var contas = await context.Contas
                .Where(c => c.UsuarioId == userId)
                .CountAsync();

            if (contas >= 5)
                throw new InvalidOperationException(message: "Limite de contas cadastradas excedido! Máximo de 05 contas por usuário.");

            var conta = new ContaEntity
            {
                Saldo = args.Saldo,
                Banco = args.Banco,
                Descricao = args.Descricao,
                Tipo = args.Tipo,
                UsuarioId = userId,
            };

            context.Contas.Add(conta);
            await context.SaveChangesAsync();

            return httpRequest.CreateResult(new { conta.Id });
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}
