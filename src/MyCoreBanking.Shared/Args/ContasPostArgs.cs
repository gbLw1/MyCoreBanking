using FluentValidation;

namespace MyCoreBanking.Args;

public class ContasPostArgs
{
    public decimal Saldo { get; set; }
    public Banco Banco { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public ContaTipo Tipo { get; set; }

    public class Validator : AbstractValidator<ContasPostArgs>
    {
        public Validator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição da conta deve ser informada")
                .MaximumLength(100).WithMessage("A descrição da conta deve ter no máximo 100 caracteres");

            RuleFor(x => x.Banco)
                .IsInEnum();

            RuleFor(x => x.Tipo)
                .IsInEnum();
        }
    }
}