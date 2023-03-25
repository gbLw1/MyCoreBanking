using FluentValidation;

namespace MyCoreBanking.Args;

public class TransacoesPostArgs
{
    public string Descricao { get; set; } = default!;
    public string? Observacao { get; set; }
    public OperacaoTipo Tipo { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataPagamento { get; set; }
    public MeioDePagamentoTipo MeioPagamento { get; set; }
    public Categoria Categoria { get; set; }
    public DateTime? InicioParcelamento { get; set; }
    public int? NumeroParcelas { get; set; }
    public decimal? ValorParcela { get; set; }

    public class Validator : AbstractValidator<TransacoesPostArgs>
    {
        public Validator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("Descrição é obrigatória");

            RuleFor(x => x.Tipo)
                .IsInEnum().WithMessage("Tipo inválido");

            RuleFor(x => x.Valor)
                .NotEmpty().WithMessage("Valor é obrigatório");

            RuleFor(x => x.MeioPagamento)
                .IsInEnum().WithMessage("Meio de pagamento inválido");

            RuleFor(x => x.Categoria)
                .IsInEnum().WithMessage("Categoria inválida");
        }
    }
}
