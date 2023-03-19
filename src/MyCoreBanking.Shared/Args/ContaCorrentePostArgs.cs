using FluentValidation;

namespace MyCoreBanking.Args;

public class ContaCorrentePostArgs
{
    public Banco Banco { get; set; }
    public string Conta { get; set; } = string.Empty;
    public string Agencia { get; set; } = string.Empty;
    public string? Observacao { get; set; }

    public class Validator : AbstractValidator<ContaCorrentePostArgs>
    {
        public Validator()
        {
            RuleFor(x => x.Banco)
            .NotEmpty().WithMessage("Banco é obrigatório");

            RuleFor(x => x.Conta)
            .NotEmpty().WithMessage("Conta é obrigatória")
            .Matches(@"^\d{5}-\d{1}$").WithMessage("Conta deve estar no formato 00000-0");

            RuleFor(x => x.Agencia)
            .NotEmpty().WithMessage("Agência é obrigatória")
            .Matches(@"^\d{4}$").WithMessage("Agência deve ter 4 dígitos");
        }
    }
}
