using MyCoreBanking.Models;

namespace MyCoreBanking.API.Data.Entities;

internal class TransacaoEntity : BaseDataEntity
{
    public string Descricao { get; set; } = default!;

    public string? Observacao { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataPagamento { get; set; }

    public UsuarioEntity? Usuario { get; set; }
    public Guid UsuarioId { get; set; }

    public MeioDePagamentoEntity? MeioDePagamento { get; set; }
    public Guid MeioDePagamentoId { get; set; }

    public TransacaoTipo Tipo { get; set; }


    public TransacaoModel ToModel() => new()
    {
        Id = Id,
        Descricao = Descricao,
        Observacao = Observacao,
        Valor = Valor,
        DataPagamento = DataPagamento,
        MeioDePagamento = MeioDePagamento?.ToModel() ?? new(),
        Tipo = Tipo,
    };
}