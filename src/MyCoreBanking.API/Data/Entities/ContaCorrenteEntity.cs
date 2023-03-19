namespace MyCoreBanking.API.Data.Entities;

internal class ContaCorrenteEntity : BaseEntity
{
    public Banco Banco { get; set; }

    public string Agencia { get; set; } = string.Empty;

    public string Conta { get; set; } = string.Empty;

    public MeioDePagamentoEntity MeioDePagamento { get; set; } = null!;
}
