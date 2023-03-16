namespace MyCoreBanking.API.Data.Entities;

internal class CartaoDeCredito : BaseEntity
{
    public string NumerosFinais { get; set; } = string.Empty;

    public Banco? Banco { get; set; }

    public BandeiraCartao Bandeira { get; set; }

    public MeioDePagamento MeioDePagamento { get; set; } = null!;
}
