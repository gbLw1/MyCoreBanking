using FluentValidation;

namespace MyCoreBanking.Args;

public class UsuariosPostArgs
{
    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

    public string ConfirmarSenha { get; set; } = string.Empty;


    public sealed class Validator : AbstractValidator<UsuariosPostArgs>
    {
        public Validator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email é obrigatório");
            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("Senha é obrigatória");
            RuleFor(x => x.ConfirmarSenha)
                .NotEmpty()
                .WithMessage("Confirmação de senha é obrigatória");
            RuleFor(x => x.Senha)
                .Equal(x => x.ConfirmarSenha)
                .WithMessage("Senhas não conferem");
        }
    }
}
