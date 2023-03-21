using FluentValidation;

namespace MyCoreBanking.Args;

public class CartoesPutArgs
{
    public string NumerosFinais { get; set; } = string.Empty;

    public Banco? Banco { get; set; }

    public BandeiraCartao Bandeira { get; set; }

    public string? Observacao { get; set; }


    public class Validator : AbstractValidator<CartoesPutArgs>
    {
        public Validator()
        {
            RuleFor(x => x.NumerosFinais)
            .NotEmpty().WithMessage("Os últimos 4 dígitos do cartão devem ser informados")
            .MaximumLength(4).WithMessage("Os últimos 4 dígitos do cartão devem ter no máximo 4 números")
            .MinimumLength(4).WithMessage("Os últimos 4 dígitos do cartão devem ter no minimo 4 números")
            .Matches(@"^\d+$").WithMessage("Os últimos 4 dígitos do cartão devem ser apenas números");

            RuleFor(x => x.Banco)
            .IsInEnum();

            RuleFor(x => x.Bandeira)
            .IsInEnum();
        }
    }
}
