using FluentValidation;

namespace MyCoreBanking.Args;

public class UsuariosPutArgs
{
    public string Nome { get; set; } = string.Empty;


    public sealed class Validator : AbstractValidator<UsuariosPutArgs>
    {
        public Validator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");
        }
    }
}
