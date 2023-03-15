namespace MyCoreBanking.API.Data.Entities;

internal sealed class ContaBancaria : BaseEntity
{
    public string TitularNome { get; set; } = default!;

    public Banco Banco { get; set; } = default!;

    public string NumeroConta { get; set; } = default!;

    public IEnumerable<Cartao>? Cartoes { get; set; }
    public IEnumerable<Transacao>? Transacoes { get; set; }
}
