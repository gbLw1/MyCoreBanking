using FluentValidation;

namespace MyCoreBanking.Args;

public class CartoesPostArgs
{
    public string NumerosFinais { get; set; } = string.Empty;

    public Banco? Banco { get; set; }

    public BandeiraCartao Bandeira { get; set; }

    /// <summary>
    /// <para>Exemplos: "Cartão de crédito do meu trabalho", "Cartão de crédito do meu pai", etc.</para>
    /// <para>Caso não seja informado, será usado o padrão: "{Bandeira} - **** **** **** {NumerosFinais}"</para>
    /// </summary>
    public string? Apelido { get; set; }


    public class Validator : AbstractValidator<CartoesPostArgs>
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
