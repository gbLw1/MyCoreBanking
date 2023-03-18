using FluentValidation;
using System.Text.Json.Serialization;

namespace MyCoreBanking.Args;

public class AuthTokenPostArgs
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("senha")]
    public string Senha { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<AuthTokenPostArgs>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email é obrigatório");
            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("Senha é obrigatória");
        }
    }
}
