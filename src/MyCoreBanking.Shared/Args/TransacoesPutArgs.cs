using FluentValidation;

namespace MyCoreBanking.Args;

public class TransacoesPutArgs
{
    public string Descricao { get; set; } = default!;

    public string? Observacao { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataPagamento { get; set; }

    public Guid MeioDePagamentoId { get; set; }

    public TransacaoTipo Tipo { get; set; }


    public sealed class Validator : AbstractValidator<TransacoesPutArgs>
    {
        public Validator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("Descrição é obrigatória")
                .MaximumLength(100).WithMessage("Descrição deve ter no máximo 100 caracteres");

            RuleFor(x => x.Observacao)
                .MaximumLength(100).WithMessage("Observação deve ter no máximo 100 caracteres");

            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("Valor deve ser maior que 0");

            RuleFor(x => x.DataPagamento)
                .NotEmpty().WithMessage("Data de pagamento é obrigatória");

            RuleFor(x => x.MeioDePagamentoId)
                .NotEmpty().WithMessage("Meio de pagamento Id é obrigatório");
        }
    }
}