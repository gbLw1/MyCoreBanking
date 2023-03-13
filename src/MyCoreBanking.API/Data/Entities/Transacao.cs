using System;

namespace MyCoreBanking.API.Data.Entities;

internal sealed class Transacao : BaseEntity
{
    public string Descricao { get; set; } = default!;

    public string? Observacao { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataTransacao { get; set; }

    public MetodoPagamento MetodoPagamento { get; set; }


    public ContaBancaria? Conta { get; set; }
}