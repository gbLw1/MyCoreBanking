using FluentValidation;

namespace MyCoreBanking.Args;

public class TransacoesPostArgs
{
    public string Descricao { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public decimal? Valor { get; set; }
    public DateTime? DataEfetivacao { get; set; } = DateTime.Now;
    public DateTime? DataTransacao { get; set; }
    public OperacaoTipo TipoOperacao { get; set; }
    public TransacaoTipo TipoTransacao { get; set; }
    public MeioPagamentoTipo MeioPagamento { get; set; }
    public Categoria Categoria { get; set; }
    public Guid ContaId { get; set; }

    /* ----------------- ↓ Transações Parceladas ↓ ---------------- */
    public DateTime? DataVencimento { get; set; }
    public int? NumeroParcelas { get; set; }
    public decimal? ValorParcela { get; set; }


    public class Validator : AbstractValidator<TransacoesPostArgs>
    {
        public Validator()
        {
            RuleFor(t => t.Descricao)
                .NotEmpty().WithMessage("A descrição da transação é obrigatória")
                .MaximumLength(100).WithMessage("A descrição da transação deve ter no máximo 100 caracteres");

            RuleFor(t => t.TipoOperacao)
                .IsInEnum().WithMessage("O tipo de operação da transação é inválido");

            RuleFor(t => t.TipoTransacao)
                .IsInEnum().WithMessage("O tipo de transação é inválido");

            RuleFor(t => t.MeioPagamento)
                .IsInEnum().WithMessage("O meio de pagamento é inválido");

            RuleFor(t => t.Categoria)
                .IsInEnum().WithMessage("A categoria é inválida");

            RuleFor(t => t.ContaId)
                .NotEmpty().WithMessage("A conta é obrigatória");

            When(t => t.TipoTransacao == TransacaoTipo.Unica, () =>
            {
                RuleFor(t => t.Valor)
                    .NotEmpty().WithMessage("O valor da transação é obrigatório")
                    .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero");

                RuleFor(t => t.DataTransacao)
                    .NotEmpty().WithMessage("A data da transação é obrigatória");
            });

            When(t => t.TipoTransacao == TransacaoTipo.Parcelada, () =>
            {
                RuleFor(t => t.DataVencimento)
                    .NotEmpty().WithMessage("A data de vencimento é obrigatória");

                RuleFor(t => t.NumeroParcelas)
                    .NotEmpty().WithMessage("O número de parcelas é obrigatório")
                    .GreaterThan(0).WithMessage("O número de parcelas deve ser maior que zero")
                    .LessThanOrEqualTo(99).WithMessage("O número de parcelas deve ser menor ou igual a 99");

                RuleFor(t => t.ValorParcela)
                    .NotEmpty().WithMessage("O valor da parcela é obrigatório")
                    .GreaterThan(0).WithMessage("O valor da parcela deve ser maior que zero");
            });
        }
    }
}
