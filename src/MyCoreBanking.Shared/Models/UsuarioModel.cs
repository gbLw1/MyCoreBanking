using System.Text.Json.Serialization;

namespace MyCoreBanking.Models;

public class UsuarioModel
{
    public Guid Id { get; set; }
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}