using System.Text.Json.Serialization;

namespace MyCoreBanking.Models;

public class MeioDePagamentoModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("apelido")]
    public string Apelido { get; set; } = string.Empty;

    [JsonPropertyName("observacao")]
    public string? Observacao { get; set; }

    [JsonPropertyName("tipo")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MeioPagamentoTipo Tipo { get; set; }
}