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




            // ↓ TRANSAÇÕES PENDENTES ↓
            var numeroTransacoesPendentesDoMesAtual = await context.Transacoes
                .AsNoTracking()
                .Where(t => t.UsuarioId == userId)
                .Where(t => t.DataEfetivacao == null)
                .Where(t => t.DataTransacao.Year <= DateTime.Now.Year)
                .Where(t => t.DataTransacao.Month <= DateTime.Now.Month)
                .CountAsync();




            // ↓ BALANÇO MENSAL ↓
            decimal balancoMensal = await context.Transacoes
                .AsNoTracking()
                .Where(t => t.UsuarioId == userId)
                .Where(t => t.DataEfetivacao != null)
                .Where(t => t.DataTransacao.Month == DateTime.Now.Month)
                .SumAsync(t => t.TipoOperacao == OperacaoTipo.Receita ? t.Valor : -t.Valor);




            // ↓ GRÁFICO DE RECEITAS E DESPESAS DO ANO ATUAL ↓
            // ! Não está em uso - Implementação requer plano de assinatura
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
                var transacao = listaReceitasDespesasAnoAtual.FirstOrDefault(g => g.Mes == i);
                if (transacao != null)
                {
                    graficoReceitasDespesasAnoAtual.Add(new GraficoDespesaReceita
                    {
                        Mes = transacao.Mes,
                        ValorDespesa = transacao.Despesa,
                        ValorReceita = transacao.Receita,
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




            // ↓ GRÁFICO DE RECEITAS E DESPESAS NOS ULTIMOS 12 MESES ↓
            var listaReceitasDespesasUltimos12meses = await context.Transacoes
                .AsNoTracking()
                .Where(t => t.UsuarioId == userId)
                .Where(t => t.DataEfetivacao != null)
                .Where(t => t.DataTransacao >= DateTime.Today.AddYears(-1))
                .GroupBy(t => t.DataTransacao.Month)
                .Select(g => new GraficoDespesaReceita
                {
                    Mes = g.Key,
                    Ano = g.Max(t => t.DataTransacao.Year),
                    ValorReceita = g.Where(t => t.TipoOperacao == OperacaoTipo.Receita).Sum(t => t.Valor),
                    ValorDespesa = g.Where(t => t.TipoOperacao == OperacaoTipo.Despesa).Sum(t => t.Valor)
                })
                .OrderBy(g => g.Ano)
                .ToListAsync();

            List<GraficoDespesaReceita> graficoReceitasDespesasUltimos12meses = new();

            var anoPassado = int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy"));
            var mesAtual = int.Parse(DateTime.Now.ToString("MM"));
            var anoAtual = int.Parse(DateTime.Now.ToString("yyyy"));

            // Percorrer todos os meses do ano passado a partir do mês atual
            for (int i = mesAtual; i <= 12; i++)
            {
                var transacao = listaReceitasDespesasUltimos12meses.FirstOrDefault(g => g.Ano == anoPassado && g.Mes == i);
                if (transacao is null)
                {
                    graficoReceitasDespesasUltimos12meses.Add(new GraficoDespesaReceita
                    {
                        Mes = i,
                        Ano = anoPassado,
                        ValorReceita = 0,
                        ValorDespesa = 0
                    });
                }
                else
                {
                    graficoReceitasDespesasUltimos12meses.Add(transacao);
                }
            }

            // Continuar percorrendo os meses porém do ano atual recomeçando em janeiro
            for (int j = 1; j <= mesAtual; j++)
            {
                var transacao = listaReceitasDespesasUltimos12meses.FirstOrDefault(g => g.Ano == anoAtual && g.Mes == j);
                if (transacao is null)
                {
                    graficoReceitasDespesasUltimos12meses.Add(new GraficoDespesaReceita
                    {
                        Mes = j,
                        Ano = anoAtual,
                        ValorReceita = 0,
                        ValorDespesa = 0
                    });
                }
                else
                {
                    graficoReceitasDespesasUltimos12meses.Add(transacao);
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
                GraficoMovimentacaoUltimos12Meses = graficoReceitasDespesasUltimos12meses,
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