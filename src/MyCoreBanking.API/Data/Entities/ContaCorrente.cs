namespace MyCoreBanking.API.Data.Entities;

internal class ContaCorrente : BaseEntity
{
    public Banco Banco { get; set; }

    public string Agencia { get; set; } = string.Empty;

    public string Conta { get; set; } = string.Empty;

    public MeioDePagamento MeioDePagamento { get; set; } = null!;
}
