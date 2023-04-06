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
using MyCoreBanking.Args;

namespace FreedomHub.API.Services.Auth;

public static class TransacoesEfetivarPut
{
    [FunctionName(nameof(TransacoesEfetivarPut))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "transacoes/{transacaoId:guid}/efetivar")] HttpRequest httpRequest,
        ILogger logger,
        Guid transacaoId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<TransacoesEfetivacaoPutArgs>();

            new TransacoesEfetivacaoPutArgs.Validator().ValidateAndThrow(args);

            // Tenta obter a transação
            var transacaoEntity = await context.Transacoes
                .Include(t => t.Conta) // include na conta para atualizar o saldo
                .Where(t => t.UsuarioId == userId)
                .FirstOrDefaultAsync(t => t.Id == transacaoId);

            if (transacaoEntity is null)
                throw new NotFoundException(message: "Transação não encontrada", paramName: nameof(transacaoId));

            if (transacaoEntity.DataEfetivacao.HasValue)
                throw new InvalidOperationException(message: "Transação já efetivada");

            // Atualização da data de efetivação
            transacaoEntity.DataEfetivacao = args.DataEfetivacao;

            // Atualização do saldo
            if (transacaoEntity.TipoOperacao == OperacaoTipo.Receita)
            {
                transacaoEntity.Conta!.Saldo += transacaoEntity.Valor;
            }
            else
            {
                transacaoEntity.Conta!.Saldo -= transacaoEntity.Valor;
            }

            context.Transacoes.Update(transacaoEntity);
            await context.SaveChangesAsync();

            return httpRequest.CreateResult(new { transacaoEntity.Id });
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}