namespace MyCoreBanking.API.Data.Entities;

internal class CartaoDeCreditoEntity : BaseEntity
{
    public string NumerosFinais { get; set; } = string.Empty;

    public Banco? Banco { get; set; }

    public BandeiraCartao Bandeira { get; set; }

    public MeioDePagamentoEntity MeioDePagamento { get; set; } = null!;
}
