using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking.API.Data;
using MyCoreBanking.Args;

namespace MyCoreBanking.API.Services.Usuarios;

public class ContasPUT
{
    [FunctionName(nameof(ContasPUT))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "contas/{contaId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid contaId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<ContasPutArgs>();

            new ContasPutArgs.Validator().ValidateAndThrow(args);

            var conta = await context.Contas
                .Where(c => c.UsuarioId == userId)
                .Where(c => c.Id == contaId)
                .FirstOrDefaultAsync();

            if (conta is null)
                throw new NotFoundException(message: "Conta não encontrada", paramName: nameof(contaId));

            conta.Saldo = args.Saldo;
            conta.Banco = args.Tipo == ContaTipo.Carteira ? Banco.Outro : args.Banco;
            conta.Descricao = args.Descricao;
            conta.Tipo = args.Tipo;

            context.Contas.Update(conta);
            await context.SaveChangesAsync();

            return httpRequest.CreateResult(new { conta.Id });
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}
