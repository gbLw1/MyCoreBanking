using MyCoreBanking.Models;

namespace MyCoreBanking.API.Data.Entities;

internal class TransacaoEntity : BaseDataEntity
{
    public string Descricao { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public TransacaoTipo Tipo { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataPagamento { get; set; }
    public MeioDePagamentoTipo MeioDePagamento { get; set; }
    public Categoria Categoria { get; set; }
    public bool Recorrente { get; set; }
    public DateTime? DataVigenciaInicio { get; set; }
    public DateTime? DataVigenciaFim { get; set; }
    public int? DiaVencimento { get; set; }
    public int? NumeroParcelas { get; set; }
    public decimal? ValorParcela { get; set; }

    public UsuarioEntity? Usuario { get; set; }
    public Guid UsuarioId { get; set; }

    public ContaEntity? Conta { get; set; }
    public Guid ContaId { get; set; }


    public TransacaoModel ToModel() => new()
    {
        Id = Id,
        Descricao = Descricao,
        Observacao = Observacao,
        Tipo = Tipo,
        Valor = Valor,
        DataPagamento = DataPagamento,
        MeioPagamento = MeioDePagamento,
        Categoria = Categoria,
        Recorrente = Recorrente,
        DataVigenciaInicio = DataVigenciaInicio,
        DataVigenciaFim = DataVigenciaFim,
        DiaVencimento = DiaVencimento,
        NumeroParcelas = NumeroParcelas,
        ValorParcela = ValorParcela
    };
}