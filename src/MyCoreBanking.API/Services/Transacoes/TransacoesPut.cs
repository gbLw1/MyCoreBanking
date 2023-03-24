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
using MyCoreBanking.Args;

namespace FreedomHub.API.Services.Auth;

public static class TransacoesPut
{
    [FunctionName(nameof(TransacoesPut))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "transacoes/{transacaoId:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid transacaoId)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<TransacoesPutArgs>();

            new TransacoesPutArgs.Validator().ValidateAndThrow(args);

            // tenta obter a transação
            var transacaoEntity = await context.Transacoes
                .Where(t => t.UsuarioId == userId)
                .FirstOrDefaultAsync(t => t.Id == transacaoId);

            if (transacaoEntity is null)
                throw new NotFoundException(message: "Transação não encontrada", paramName: nameof(transacaoId));

            // // tenta obter o meio de pagamento
            // var meioDePagamento = await context.MeiosDePagamento.FirstOrDefaultAsync(m => m.Id == args.MeioDePagamentoId);

            // if (meioDePagamento is null)
            //     throw new NotFoundException(paramName: "Meio de pagamento");

            // if (meioDePagamento.UsuarioId != userId)
            //     throw new UnauthorizedAccessException("Meio de pagamento não pertence ao usuário logado.");

            // if (args.Tipo == TransacaoTipo.Receita && meioDePagamento.Tipo != MeioDePagamentoTipo.ContaCorrente)
            //     throw new InvalidOperationException("Tipo de transação inválida para o tipo de meio de pagamento selecionado.");

            // // atualizar a transação
            // transacaoEntity.Descricao = args.Descricao;
            // transacaoEntity.Observacao = args.Observacao;
            // transacaoEntity.Valor = args.Valor;
            // transacaoEntity.DataPagamento = args.DataPagamento;
            // transacaoEntity.MeioDePagamentoId = args.MeioDePagamentoId;
            // transacaoEntity.Tipo = args.Tipo;

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