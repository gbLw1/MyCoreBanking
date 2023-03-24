namespace MyCoreBanking.API.Data.Entities;

internal sealed class UsuarioEntity : BaseDataEntity
{
    public string Nome { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string SenhaHash { get; set; } = default!;

    public ICollection<TransacaoEntity>? Transacoes { get; set; }

    public ICollection<ContaEntity>? Contas { get; set; }

    // public ICollection<CartaoDeCreditoEntity>? CartoesDeCredito { get; set; }


    public bool SenhaValida(string senha)
    {
        return BCrypt.Net.BCrypt.Verify(senha, SenhaHash);
    }

    public void HashSenha(string senha)
    {
        SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
    }
}
