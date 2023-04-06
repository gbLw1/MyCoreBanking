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
using MyCoreBanking.Models;

namespace FreedomHub.API.Services.Auth;

public static class RelatoriosGet
{
    [FunctionName(nameof(RelatoriosGet))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "relatorios")] HttpRequest httpRequest,
        ILogger logger)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            // ↓ SALDO TOTAL DE TODAS AS CONTAS ↓
            var saldoTotal = await context.Contas
                .AsNoTracking()
                .Where(c => c.UsuarioId == userId)
                .Where(c => c.Tipo == ContaTipo.Corrente || c.Tipo == ContaTipo.Carteira)
                .SumAsync(c => c.Saldo);

            // ↓ TOTAL INVESTIDO ↓
            var totalInvestido = await context.Contas
                .AsNoTracking()
                .Where(c => c.UsuarioId == userId)
                .Where(c => c.Tipo == ContaTipo.Investimento || c.Tipo == ContaTipo.Poupanca)
                .SumAsync(c => c.Saldo);

            // ↓ TRANSAÇÕES PENDENTES DO MÊS ATUAL ↓
            var numeroTransacoesPendentesDoMesAtual = await context.Transacoes
                .AsNoTracking()
                .Where(t => t.UsuarioId == userId)
                .Where(t => t.DataEfetivacao == null)
                .Where(t => t.DataTransacao.Month == DateTime.Now.Month)
                .CountAsync();

            // ↓ BALANÇO MENSAL ↓
            decimal balancoMensal = await context.Transacoes
                .AsNoTracking()
                .Where(t => t.UsuarioId == userId)
                .Where(t => t.DataEfetivacao != null)
                .Where(t => t.DataTransacao.Month == DateTime.Now.Month)
                .SumAsync(t => t.TipoOperacao == OperacaoTipo.Receita ? t.Valor : -t.Valor);

            // ↓ GRÁFICO DE RECEITAS E DESPESAS DO ANO ATUAL ↓
            var listaReceitasDespesasAnoAtual = await context.Transacoes
                .AsNoTracking()
                .Where(t => t.UsuarioId == userId)
                .Where(t => t.DataEfetivacao != null)
                .Where(t => t.DataTransacao.Year == DateTime.Now.Year)
                .GroupBy(t => t.DataTransacao.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Receita = g.Where(t => t.TipoOperacao == OperacaoTipo.Receita).Sum(t => t.Valor),
                    Despesa = g.Where(t => t.TipoOperacao == OperacaoTipo.Despesa).Sum(t => t.Valor)
                })
                .ToListAsync();

            List<GraficoDespesaReceita> graficoReceitasDespesasAnoAtual = new();

            for (int i = 1; i <= 12; i++)
            {
                var graph = listaReceitasDespesasAnoAtual.FirstOrDefault(g => g.Mes == i);
                if (graph != null)
                {
                    graficoReceitasDespesasAnoAtual.Add(new GraficoDespesaReceita
                    {
                        Mes = graph.Mes,
                        ValorDespesa = graph.Despesa,
                        ValorReceita = graph.Receita,
                    });
                }
                else
                {
                    graficoReceitasDespesasAnoAtual.Add(new GraficoDespesaReceita
                    {
                        Mes = i,
                        ValorDespesa = 0,
                        ValorReceita = 0,
                    });
                }
            }

            // ↓ GRÁFICO DE DESPESAS POR CATEGORIA MENSAL ↓
            var graficoTotalDespesasPorCategoriaMensal = await context.Transacoes
                .AsNoTracking()
                .Where(t => t.UsuarioId == userId)
                .Where(t => t.DataEfetivacao != null)
                .Where(t => t.DataTransacao.Year == DateTime.Now.Year)
                .Where(t => t.DataTransacao.Month == DateTime.Now.Month)
                .Where(t => t.TipoOperacao == OperacaoTipo.Despesa)
                .GroupBy(t => t.Categoria)
                .Select(g => new GraficoDespesaPorCategoria { Categoria = g.Key, Valor = g.Sum(t => t.Valor) })
                .ToListAsync();

            // ↓ GRÁFICO DE DESPESAS POR CATEGORIA ANUAL ↓
            var graficoTotalDespesasPorCategoriaAnual = await context.Transacoes
                .AsNoTracking()
                .Where(t => t.UsuarioId == userId)
                .Where(t => t.DataEfetivacao != null)
                .Where(t => t.DataTransacao.Year == DateTime.Now.Year)
                .Where(t => t.TipoOperacao == OperacaoTipo.Despesa)
                .GroupBy(t => t.Categoria)
                .Select(g => new GraficoDespesaPorCategoria { Categoria = g.Key, Valor = g.Sum(t => t.Valor) })
                .ToListAsync();


            RelatorioModel relatorio = new()
            {
                SaldoTotal = saldoTotal,
                TotalInvestido = totalInvestido,
                TransacoesPendentes = numeroTransacoesPendentesDoMesAtual,
                BalancoMensal = balancoMensal,
                GraficoMovimentacaoAnoAtual = graficoReceitasDespesasAnoAtual,
                GraficoDespesaPorCategoriaMensal = graficoTotalDespesasPorCategoriaMensal,
                GraficoDespesaPorCategoriaAnual = graficoTotalDespesasPorCategoriaAnual,
            };

            return httpRequest.CreateResult(relatorio);
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}