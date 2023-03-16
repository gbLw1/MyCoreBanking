namespace MyCoreBanking.API.Data.Entities;

internal class MeioDePagamentoCartaoDeCredito : BaseEntity
{
    public string NumerosFinais { get; set; } = string.Empty;

    // TODO: Criar enum para bandeiras
    public string Bandeira { get; set; } = string.Empty;

    public MeioDePagamento? MeioDePagamento { get; set; }
}
