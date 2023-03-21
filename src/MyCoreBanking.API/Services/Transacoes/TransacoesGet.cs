using System.Globalization;
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

            var queryParameters = httpRequest.GetQueryParameterDictionary();

            IQueryable<TransacaoEntity> query = context.Transacoes
                .AsNoTrackingWithIdentityResolution()
                .Include(t => t.MeioDePagamento)
                .Where(t => t.UsuarioId == userId)
                .OrderByDescending(t => t.DataPagamento);

            // Filtro por "meioDePagamentoId"
            if (queryParameters.TryGetValue("meioDePagamentoId", out var meioDePagamentoId)
                && Guid.TryParse(meioDePagamentoId, out var meioDePagamentoIdGuid))
            {
                query = query.Where(t => t.MeioDePagamentoId == meioDePagamentoIdGuid);
            }

            // Filtro por enum "meio de pagamento"
            if (queryParameters.TryGetValue("meioDePagamento", out var meioDePagamento)
                && Enum.TryParse<MeioDePagamentoTipo>(meioDePagamento, out var meioDePagamentoEnum))
            {
                query = query.Where(t => t.MeioDePagamento!.Tipo == meioDePagamentoEnum);
            }

            // Filtro por enum "tipo de transação"
            if (queryParameters.TryGetValue("tipoDeTransacao", out var tipoDeTransacao)
                && Enum.TryParse<TransacaoTipo>(tipoDeTransacao, out var tipoDeTransacaoEnum))
            {
                query = query.Where(t => t.Tipo == tipoDeTransacaoEnum);
            }

            // Filtro por período de data
            string[] formatosDeData = { "dd-MM-yyyy", "dd/MM/yyyy" };
            CultureInfo ptBR = new("pt-BR");
            DateTime dataInicial = default;
            DateTime dataFinal = default;

            // Filtro por "dataInicial"
            if (queryParameters.TryGetValue("dataInicial", out string? dataInicialString)
                && DateTime.TryParseExact(dataInicialString, formatosDeData, ptBR, DateTimeStyles.AssumeLocal, out dataInicial))
            {
                query = query.Where(t => t.DataPagamento >= dataInicial);
            }

            // Filtro por "dataFinal"
            if (queryParameters.TryGetValue("dataFinal", out string? dataFinalString)
                && DateTime.TryParseExact(dataFinalString, formatosDeData, ptBR, DateTimeStyles.AssumeLocal, out dataFinal))
            {
                query = query.Where(t => t.DataPagamento <= dataFinal);
            }

            if (dataInicial != default && dataFinal != default
                && dataInicial.CompareTo(dataFinal) > 0)
            {
                throw new ArgumentException("A data inicial não pode ser maior que a data final.");
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