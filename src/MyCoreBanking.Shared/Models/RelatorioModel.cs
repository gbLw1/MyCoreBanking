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

    [JsonPropertyName("GraficoDespesaReceitaDoAnoAtual")]
    public List<GraficoDespesaReceita>? GraficoDespesaReceitaDoAnoAtual { get; set; }

    [JsonPropertyName("graficoDespesaPorCategoria")]
    public List<GraficoDespesaPorCategoria>? GraficoDespesaPorCategoria { get; set; }
}

public class GraficoDespesaReceita
{
    [JsonPropertyName("mes")]
    public int Mes { get; set; }

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