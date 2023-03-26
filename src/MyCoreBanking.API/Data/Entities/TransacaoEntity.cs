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

    /// <summary>
    /// <para>Indica o tipo de operação da transação: entrada ou saída</para>
    /// </summary>
    public OperacaoTipo TipoDeOperacao { get; set; }

    /// <summary>
    /// <para>Indica o tipo de transação: recorrente, parcelada ou única</para>
    /// </summary>
    public TransacaoTipo TipoDeTransacao { get; set; }

    public MeioDePagamentoTipo MeioDePagamento { get; set; }

    public Categoria Categoria { get; set; }


    /* ----------------- ↓ Transações Parceladas ↓ ---------------- */

    /// <summary>
    /// <para>Obrigatório quando Parcelamento</para>
    /// <para>Data de quando inicia um parcelamento</para>
    /// </summary>
    public DateTime? InicioParcelamento { get; set; }

    /// <summary>
    /// <para>Obrigatório quando Parcelamento</para>
    /// <para>Dia limite para efetivar o pagamento, usado para identificar atrasos</para>
    /// </summary>
    public DateTime? DataVencimento { get; set; }

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
        Valor = Valor,
        DataPagamento = DataPagamento,
        TipoDeOperacao = TipoDeOperacao,
        TipoDeTransacao = TipoDeTransacao,
        MeioDePagamento = MeioDePagamento,
        Categoria = Categoria,
        InicioParcelamento = InicioParcelamento,
        DataVencimento = DataVencimento,
        NumeroParcelas = NumeroParcelas,
        ValorParcela = ValorParcela,
    };
}