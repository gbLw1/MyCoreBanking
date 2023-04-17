using System.Text.Json.Serialization;

namespace MyCoreBanking.Models;

public class RelatorioModel
{
    [JsonPropertyName("saldoTotal")]
    public decimal SaldoTotal { get; set; }

    [JsonPropertyName("totalInvestido")]
    public decimal TotalInvestido { get; set; }

    [JsonPropertyName("transacoesPendentes")]
    public int TransacoesPendentes { get; set; }

    [JsonPropertyName("balancoMensal")]
    public decimal BalancoMensal { get; set; }

    [JsonPropertyName("graficoMovimentacaoAnoAtual")]
    public List<GraficoDespesaReceita>? GraficoMovimentacaoAnoAtual { get; set; }

    [JsonPropertyName("graficoMovimentacaoUltimos12Meses")]
    public List<GraficoDespesaReceita>? GraficoMovimentacaoUltimos12Meses { get; set; }

    [JsonPropertyName("graficoDespesaPorCategoriaMensal")]
    public List<GraficoDespesaPorCategoria>? GraficoDespesaPorCategoriaMensal { get; set; }

    [JsonPropertyName("graficoDespesaPorCategoriaAnual")]
    public List<GraficoDespesaPorCategoria>? GraficoDespesaPorCategoriaAnual { get; set; }

}

public class GraficoDespesaReceita
{
    [JsonPropertyName("mes")]
    public int Mes { get; set; }

    [JsonPropertyName("ano")]
    public int Ano { get; set; }

    [JsonPropertyName("valorDespesa")]
    public decimal ValorDespesa { get; set; }

    [JsonPropertyName("valorReceita")]
    public decimal ValorReceita { get; set; }
}

public class GraficoDespesaPorCategoria
{
    [JsonPropertyName("categoria")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Categoria Categoria { get; set; }

    [JsonPropertyName("valor")]
    public decimal Valor { get; set; }
}