using MyCoreBanking.Models;

namespace MyCoreBanking.API.Data.Entities;

internal class ContaCorrenteEntity : BaseEntity
{
    public Banco Banco { get; set; }

    public string Agencia { get; set; } = string.Empty;

    public string Conta { get; set; } = string.Empty;

    public MeioDePagamentoEntity MeioDePagamento { get; set; } = null!;


    public ContaCorrenteModel ToModel() => new()
    {
        Id = MeioDePagamento.Id,
        Apelido = MeioDePagamento.Apelido,
        Observacao = MeioDePagamento.Observacao,
        Tipo = MeioDePagamento.Tipo,
        Banco = Banco,
        Agencia = Agencia,
        Conta = Conta,
    };
}
