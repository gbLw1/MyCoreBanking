using MyCoreBanking.Models;

namespace MyCoreBanking.API.Data.Entities;

internal class TransacaoEntity : BaseDataEntity
{
    public string Descricao { get; set; } = string.Empty;

    public string? Observacao { get; set; }

    public decimal Valor { get; set; }

    /// <summary>
    /// <para>Identifica a data de pagamento da transação</para>
    /// <para>Quando não informado, é considerado como uma transação pendente</para>
    /// </summary>
    public DateTime? DataPagamento { get; set; }

    public TransacaoTipo Tipo { get; set; }

    public MeioDePagamentoTipo MeioDePagamento { get; set; }

    public Categoria Categoria { get; set; }


    /* ------------------------ ↓ Transação recorrente ↓ ------------------------ */

    /// <summary>
    /// <para>Obrigatório quando Recorrente</para>
    /// </summary>
    public bool Recorrente { get; set; }

    /// <summary>
    /// <para>Obrigatório quando Recorrente ou Parcelamento</para>
    /// <para>Data de quando uma transação começa a ser recorrente ou inicia um parcelamento</para>
    /// </summary>
    public DateTime? DataVigenciaInicio { get; set; }

    /// <summary>
    /// <para>Opcional para transação recorrente</para>
    /// <para>Identifica o fim de uma transação recorrente, deixando de aparecer no extrato a partir desta data</para>
    /// </summary>
    public DateTime? DataVigenciaFim { get; set; }

    /// <summary>
    /// <para>Obrigatório quando Recorrente ou Parcelamento</para>
    /// <para>Dia limite para efetivar o pagamento, usado para identificar atrasos</para>
    /// </summary>
    public int? DiaVencimento { get; set; }

    /// <summary>
    /// <para>Obrigatório quando Parcelamento</para>
    /// <para>Quantidade que irá repetir ao cadastrar a transação num intervalo de meses</para>
    /// </summary>
    public int? NumeroParcelas { get; set; }

    /// <summary>
    /// <para>Obrigatório quando Parcelamento</para>
    /// </summary>
    public decimal? ValorParcela { get; set; }


    /* --------------------------------- ↓ FKs ↓ -------------------------------- */

    public UsuarioEntity? Usuario { get; set; }
    public Guid UsuarioId { get; set; }

    public ContaEntity? Conta { get; set; }
    public Guid ContaId { get; set; }


    /* ----------------------------- ↓ MapToModel ↓ ----------------------------- */

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