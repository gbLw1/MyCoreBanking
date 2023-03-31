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
using System.Globalization;

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
                .Include(t => t.Conta)
                .Where(t => t.UsuarioId == userId)
                .OrderByDescending(t => t.DataEfetivacao);


            // ↓ parâmetros obrigatórios para obter as transações ↓

            if (queryParameters.TryGetValue("parcelamentoId", out var parcelamentoIdStr)
                && Guid.TryParse(parcelamentoIdStr, out var parcelamentoId))
            {
                query = query.Where(t => t.ReferenciaParcelaId == parcelamentoId);
            }
            else
            {
                if (!queryParameters.TryGetValue("mes", out var mes)
                    || !int.TryParse(mes, NumberStyles.Integer, CultureInfo.InvariantCulture, out var mesInt)
                    || mesInt < 1 || mesInt > 12)
                {
                    throw new InvalidOperationException(message: "O parâmetro 'mes' é obrigatório e deve ser um número entre 1 e 12");
                }

                if (!queryParameters.TryGetValue("ano", out var ano)
                    || !int.TryParse(ano, NumberStyles.Integer, CultureInfo.InvariantCulture, out var anoInt)
                    || anoInt < 2000 || anoInt > 2100)
                {
                    throw new InvalidOperationException(message: "O parâmetro 'ano' é obrigatório e deve ser um número entre 2000 e 2100");
                }

                // busca todas as transações com a data de transação ou data de vencimento no mês e ano informados
                query = query.Where(t =>
                        (t.TipoTransacao == TransacaoTipo.Parcelada
                        && t.DataVencimento!.Value.Month == mesInt
                        && t.DataVencimento.Value.Year == anoInt)
                    || (t.TipoTransacao == TransacaoTipo.Unica
                        && t.DataTransacao.Month == mesInt
                        && t.DataTransacao.Year == anoInt));
            }

            #region [Outros filtros de pesquisa por query params]

            // Filtro por enum "meio de pagamento"
            if (queryParameters.TryGetValue("meioDePagamento", out var meioDePagamento)
                && Enum.TryParse<MeioPagamentoTipo>(meioDePagamento, out var meioDePagamentoEnum))
            {
                query = query.Where(t => t.MeioPagamento == meioDePagamentoEnum);
            }

            // Filtro por enum "tipo de operação" (receita ou despesa)
            if (queryParameters.TryGetValue("tipoDeOperacao", out var tipoDeOperacao)
                && Enum.TryParse<OperacaoTipo>(tipoDeOperacao, out var tipoDeOperacaoEnum))
            {
                query = query.Where(t => t.TipoOperacao == tipoDeOperacaoEnum);
            }

            // Filtro por enum "tipo de transação" (parcelada ou única)
            if (queryParameters.TryGetValue("tipoDeTransacao", out var tipoDeTransacao)
                && Enum.TryParse<TransacaoTipo>(tipoDeTransacao, out var tipoDeTransacaoEnum))
            {
                query = query.Where(t => t.TipoTransacao == tipoDeTransacaoEnum);
            }

            // Filtro por enum "categoria"
            if (queryParameters.TryGetValue("categoria", out var categoria)
                && Enum.TryParse<Categoria>(categoria, out var categoriaEnum))
            {
                query = query.Where(t => t.Categoria == categoriaEnum);
            }

            // Filtro por período de data de pagamento
            string[] formatosDeData = { "dd-MM-yyyy", "dd/MM/yyyy" };
            CultureInfo ptBR = new("pt-BR");
            DateTime dataInicial = default;
            DateTime dataFinal = default;

            // Filtro por "dataInicial"
            if (queryParameters.TryGetValue("dataInicial", out string? dataInicialString)
                && DateTime.TryParseExact(dataInicialString, formatosDeData, ptBR, DateTimeStyles.AssumeLocal, out dataInicial))
            {
                query = query.Where(t => t.DataEfetivacao >= dataInicial);
            }

            // Filtro por "dataFinal"
            if (queryParameters.TryGetValue("dataFinal", out string? dataFinalString)
                && DateTime.TryParseExact(dataFinalString, formatosDeData, ptBR, DateTimeStyles.AssumeLocal, out dataFinal))
            {
                query = query.Where(t => t.DataEfetivacao <= dataFinal);
            }

            // Validação de "dataInicial" e "dataFinal"
            if (dataInicial != default && dataFinal != default
                && dataInicial.CompareTo(dataFinal) > 0)
            {
                throw new ArgumentException("A data inicial não pode ser maior que a data final.");
            }

            #endregion

            var transacoes = await query
                .Select(t => t.ToModel())
                .ToListAsync();

            var result = transacoes.AsReadOnly();

            return httpRequest.CreateResult(result);
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}