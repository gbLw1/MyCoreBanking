using FluentValidation;

namespace MyCoreBanking.Args;

public class TransacoesPutArgs
{
    public string Descricao { get; set; } = default!;
    public string? Observacao { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataEfetivacao { get; set; }
    public DateTime? DataTransacao { get; set; }
    public MeioPagamentoTipo? MeioPagamento { get; set; }
    public Categoria? Categoria { get; set; }


    public class Validator : AbstractValidator<TransacoesPutArgs>
    {
        public Validator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição da transação não pode ser vazia");

            RuleFor(x => x.Valor).
                GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero");

            When(x => x.Categoria.HasValue, () =>
            {
                RuleFor(x => x.Categoria).IsInEnum();
            });
        }
    }
}
