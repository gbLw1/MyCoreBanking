using FluentValidation;

namespace MyCoreBanking.Args;

public class ContaCorrentePostArgs
{
    /// <summary>
    /// <para>Exemplos: "Pix", "Transferência", "Investimento"</para>
    /// </summary>
    public string FormaDePagamento { get; set; } = string.Empty;
    public Banco Banco { get; set; }
    public string Conta { get; set; } = string.Empty;
    public string Agencia { get; set; } = string.Empty;


    public class Validator : AbstractValidator<ContaCorrentePostArgs>
    {
        public Validator()
        {
            RuleFor(c => c.FormaDePagamento)
                .NotEmpty().WithMessage("Apelido é obrigatório");

            RuleFor(c => c.Banco)
                .IsInEnum().WithMessage("Banco inválido");

            RuleFor(c => c.Conta)
                .NotEmpty().WithMessage("Conta é obrigatória")
                .Matches(@"^\d{5}-\d{1}$").WithMessage("Conta deve estar no formato 00000-0");

            RuleFor(c => c.Agencia)
                .NotEmpty().WithMessage("Agência é obrigatória")
                .Matches(@"^\d{4}$").WithMessage("Agência deve ter 4 dígitos");
        }
    }
}
