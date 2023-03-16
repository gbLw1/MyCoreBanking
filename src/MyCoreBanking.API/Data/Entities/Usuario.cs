namespace MyCoreBanking.API.Data.Entities;

internal sealed class Usuario : BaseDataEntity
{
    public string Nome { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Senha { get; set; } = default!;

    public ICollection<Transacao>? Transacoes { get; set; }
    public ICollection<MeioDePagamento>? MeiosDePagamento { get; set; }

    public ICollection<ContaCorrente>? ContaCorrentes { get; set; }
    public ICollection<CartaoDeCredito>? CartaoDeCreditos { get; set; }
}