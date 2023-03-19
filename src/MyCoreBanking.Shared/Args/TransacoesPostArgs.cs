using FluentValidation;
using System.Text.Json.Serialization;

namespace MyCoreBanking.Args;

public class TransacoesPostArgs
{
    public string Descricao { get; set; } = default!;

    public string? Observacao { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataPagamento { get; set; }

    public Guid MeioDePagamentoId { get; set; }

    public TransacaoTipo Tipo { get; set; }


    public sealed class Validator : AbstractValidator<TransacoesPostArgs>
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
                .NotEmpty().WithMessage("Meio de pagamento é obrigatório");
        }
    }
}
