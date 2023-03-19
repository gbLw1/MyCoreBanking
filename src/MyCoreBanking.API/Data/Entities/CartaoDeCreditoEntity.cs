using MyCoreBanking.Models;

namespace MyCoreBanking.API.Data.Entities;

internal class CartaoDeCreditoEntity : BaseEntity
{
    public string NumerosFinais { get; set; } = string.Empty;

    public Banco? Banco { get; set; }

    public BandeiraCartao Bandeira { get; set; }

    public MeioDePagamentoEntity MeioDePagamento { get; set; } = null!;


    public CartaoDeCreditoModel ToModel() => new()
    {
        Id = Id,
        Apelido = MeioDePagamento.Apelido,
        Observacao = MeioDePagamento.Observacao,
        Tipo = MeioDePagamento.Tipo,
        NumerosFinais = NumerosFinais,
        Banco = Banco,
        Bandeira = Bandeira,
    };
}
