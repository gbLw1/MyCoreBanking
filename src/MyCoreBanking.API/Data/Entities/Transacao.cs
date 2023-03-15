namespace MyCoreBanking.API.Data.Entities;

internal sealed class Transacao : BaseEntity
{
    public string Descricao { get; set; } = default!;

    public string? Observacao { get; set; }

    public decimal Valor { get; set; }

    public DateTime? DataPagamento { get; set; }

    public DateTime? DataVencimento { get; set; }

    public MetodoPagamento MetodoPagamento { get; set; }


    public ContaBancaria? Conta { get; set; }
}