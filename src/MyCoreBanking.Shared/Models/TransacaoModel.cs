using System.Text.Json.Serialization;

namespace MyCoreBanking.Models;

public class TransacaoModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = string.Empty;

    [JsonPropertyName("observacao")]
    public string? Observacao { get; set; }

    [JsonPropertyName("valor")]
    public decimal Valor { get; set; }

    [JsonPropertyName("efetivada")]
    public bool Efetivada => DataEfetivacao.HasValue;

    [JsonPropertyName("dataEfetivacao")]
    public DateTime? DataEfetivacao { get; set; }

    [JsonPropertyName("dataTransacao")]
    public DateTime DataTransacao { get; set; }

    [JsonPropertyName("tipoOperacao")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OperacaoTipo TipoOperacao { get; set; }

    [JsonPropertyName("tipoTransacao")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransacaoTipo TipoTransacao { get; set; }

    [JsonPropertyName("meioPagamento")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MeioPagamentoTipo MeioPagamento { get; set; }

    [JsonPropertyName("categoria")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Categoria Categoria { get; set; }

    [JsonPropertyName("parcelaAtual")]
    public int? ParcelaAtual { get; set; }

    [JsonPropertyName("numeroParcelas")]
    public int? NumeroParcelas { get; set; }

    [JsonPropertyName("referenciaParcelaId")]
    public Guid? ReferenciaParcelaId { get; set; }

    [JsonPropertyName("conta")]
    public string Conta { get; set; } = string.Empty;
}