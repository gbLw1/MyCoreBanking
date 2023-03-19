using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking;
using MyCoreBanking.API.Data;
using MyCoreBanking.API.Data.Entities;
using MyCoreBanking.Args;

namespace FreedomHub.API.Services.Auth;

public static class ContasPost
{
    [FunctionName(nameof(ContasPost))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "contas")] HttpRequest httpRequest,
        ILogger logger)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<ContaCorrentePostArgs>();

            new ContaCorrentePostArgs.Validator().ValidateAndThrow(args);

            var contaCorrenteEntity = new ContaCorrenteEntity
            {
                Conta = args.Conta,
                Banco = args.Banco,
                Agencia = args.Agencia,
                MeioDePagamento = new MeioDePagamentoEntity
                {
                    Apelido = $"{args.Banco}_{args.Conta}",
                    Observacao = args.Observacao,
                    Tipo = MeioDePagamentoTipo.ContaCorrente,
                    UsuarioId = userId,
                },
            };

            context.ContasCorrente.Add(contaCorrenteEntity);

            await context.SaveChangesAsync();

            return httpRequest.CreateResult(new { contaCorrenteEntity.Id });
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}