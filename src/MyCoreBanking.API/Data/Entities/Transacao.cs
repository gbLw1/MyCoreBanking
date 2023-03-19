using MyCoreBanking.Models;

namespace MyCoreBanking.API.Data.Entities;

internal class Transacao : BaseDataEntity
{
    public string Descricao { get; set; } = default!;

    public string? Observacao { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataPagamento { get; set; }

    public Usuario? Usuario { get; set; }
    public Guid UsuarioId { get; set; }

    public MeioDePagamento? MeioDePagamento { get; set; }
    public Guid MeioDePagamentoId { get; set; }


    public TransacaoModel ToModel() => new()
    {
        Id = Id,
        Descricao = Descricao,
        Observacao = Observacao,
        Valor = Valor,
        DataPagamento = DataPagamento,
        MeioDePagamentoId = MeioDePagamentoId,
    };
}