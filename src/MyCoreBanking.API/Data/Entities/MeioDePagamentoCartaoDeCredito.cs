namespace MyCoreBanking.API.Data.Entities;

internal class MeioDePagamentoCartaoDeCredito : BaseEntity
{
    public string NumerosFinais { get; set; } = string.Empty;

    public Banco? Banco { get; set; }

    // TODO: Criar enum para bandeiras
    public string Bandeira { get; set; } = string.Empty;

    public MeioDePagamento MeioDePagamento { get; set; } = null!;
}
