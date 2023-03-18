namespace MyCoreBanking.API.Data.Entities;

internal sealed class Usuario : BaseDataEntity
{
    public string Nome { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string SenhaHash { get; set; } = default!;

    public ICollection<Transacao>? Transacoes { get; set; }
    public ICollection<MeioDePagamento>? MeiosDePagamento { get; set; }

    public ICollection<ContaCorrente>? ContasCorrente { get; set; }
    public ICollection<CartaoDeCredito>? CartoesDeCredito { get; set; }

    public bool VerificarSenha(string senha)
    {
        return BCrypt.Net.BCrypt.Verify(senha, SenhaHash);
    }

    public void HashSenha(string senha)
    {
        SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
    }
}
