using System.Text.Json.Serialization;

namespace MyCoreBanking.Models;

public class ContaModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("saldo")]
    public decimal Saldo { get; set; }

    [JsonPropertyName("banco")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Banco Banco { get; set; }

    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = string.Empty;

    // json converter to string
    [JsonPropertyName("tipo")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ContaTipo Tipo { get; set; }
}