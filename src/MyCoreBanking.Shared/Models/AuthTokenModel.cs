using System.Text.Json.Serialization;

namespace MyCoreBanking.Models;

public class AuthTokenModel
{
    [JsonPropertyName("tokenType")]
    public string? TokenType { get; set; }

    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("expiresIn")]
    public string? ExpiresIn { get; set; }
}