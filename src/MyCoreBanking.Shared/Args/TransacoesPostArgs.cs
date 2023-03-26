using FluentValidation;

namespace MyCoreBanking.Args;

public class TransacoesPostArgs
{
    public string Descricao { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public decimal? Valor { get; set; }
    public DateTime? DataPagamento { get; set; }
    public OperacaoTipo TipoDeOperacao { get; set; }
    public TransacaoTipo TipoDeTransacao { get; set; }
    public MeioDePagamentoTipo MeioDePagamento { get; set; }
    public Categoria Categoria { get; set; }
    public Guid ContaId { get; set; }

    /* ----------------- ↓ Transações Parceladas ↓ ---------------- */
    public DateTime? InicioParcelamento { get; set; }
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

            When(t => t.TipoDeTransacao == TransacaoTipo.Unica, () =>
            {
                RuleFor(t => t.Valor)
                    .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero");
            });

            RuleFor(t => t.TipoDeOperacao)
                .IsInEnum().WithMessage("O tipo de operação da transação é inválido");

            RuleFor(t => t.TipoDeTransacao)
                .IsInEnum().WithMessage("O tipo de transação é inválido");

            RuleFor(t => t.MeioDePagamento)
                .IsInEnum().WithMessage("O meio de pagamento é inválido");

            RuleFor(t => t.Categoria)
                .IsInEnum().WithMessage("A categoria é inválida");

            RuleFor(t => t.ContaId)
                .NotEmpty().WithMessage("A conta é obrigatória");

            When(t => t.TipoDeTransacao == TransacaoTipo.Parcelada, () =>
            {
                RuleFor(t => t.InicioParcelamento)
                    .NotEmpty().WithMessage("A data de início do parcelamento é obrigatória");

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
