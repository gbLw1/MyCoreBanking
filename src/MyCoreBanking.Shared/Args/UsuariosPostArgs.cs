using System.Text.Json.Serialization;

namespace MyCoreBanking.Args;

public class UsuariosPostArgs
{
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("senha")]
    public string Senha { get; set; } = string.Empty;

    [JsonPropertyName("confirmarSenha")]
    public string ConfirmarSenha { get; set; } = string.Empty;
}
