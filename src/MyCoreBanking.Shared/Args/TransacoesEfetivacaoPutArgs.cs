using FluentValidation;

namespace MyCoreBanking.Args;

public class TransacoesEfetivacaoPutArgs
{
    public DateTime DataEfetivacao { get; set; }


    public class Validator : AbstractValidator<TransacoesEfetivacaoPutArgs>
    {
        public Validator()
        {
            RuleFor(t => t.DataEfetivacao)
                .NotEmpty().WithMessage("A data de efetivação da transação não pode ser vazia");
        }
    }
}
