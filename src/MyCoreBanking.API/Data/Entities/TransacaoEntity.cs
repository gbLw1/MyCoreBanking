using MyCoreBanking.Models;

namespace MyCoreBanking.API.Data.Entities;

internal class TransacaoEntity : BaseDataEntity
{
    public string Descricao { get; set; } = string.Empty;

    public string? Observacao { get; set; }

    public decimal Valor { get; set; }

    /// <summary>
    /// <para>Identifica a data de pagamento/recebimento de uma transação</para>
    /// <para>Quando não informado, é considerado como uma transação pendente</para>
    /// </summary>
    public DateTime? DataEfetivacao { get; set; }

    /// <summary>
    /// <para>Transação única: identificar quando ocorreu a transação</para>
    /// <para>Transação parcelada: identificar quando inicia um parcelamento</para>
    /// </summary>
    public DateTime DataTransacao { get; set; }

    /// <summary>
    /// <para>Indica o tipo de operação da transação: entrada ou saída</para>
    /// </summary>
    public OperacaoTipo TipoOperacao { get; set; }

    /// <summary>
    /// <para>Indica o tipo de transação: recorrente, parcelada ou única</para>
    /// </summary>
    public TransacaoTipo TipoTransacao { get; set; }

    public MeioPagamentoTipo MeioPagamento { get; set; }

    public Categoria Categoria { get; set; }


    /* ----------------- ↓ Transações Parceladas ↓ ---------------- */

    /// <summary>
    /// <para>Id referente a transação que foi parcelada</para>
    /// <para>Usado para identificar todas as parcelas da transação original</para>
    /// </summary>
    public Guid? ReferenciaParcelaId { get; set; }

    /// <summary>
    /// <para>Obrigatório quando Parcelamento</para>
    /// <para>Dia limite para efetivar o pagamento, usado para identificar atrasos</para>
    /// </summary>
    public DateTime? DataVencimento { get; set; }

    public int? ParcelaAtual { get; set; }

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
        DataEfetivacao = DataEfetivacao,
        DataTransacao = DataTransacao,
        TipoOperacao = TipoOperacao,
        TipoTransacao = TipoTransacao,
        MeioPagamento = MeioPagamento,
        Categoria = Categoria,
        DataVencimento = DataVencimento,
        ParcelaAtual = ParcelaAtual,
        NumeroParcelas = NumeroParcelas,
        ValorParcela = ValorParcela,
        ReferenciaParcelaId = ReferenciaParcelaId,
    };
}