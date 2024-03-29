using MyCoreBanking.Models;

namespace MyCoreBanking.API.Data.Entities;

internal class TransacaoEntity : BaseDataEntity
{
    public string Descricao { get; set; } = string.Empty;

    public string? Observacao { get; set; }

    /// <summary>
    /// <para>Valor total quando a transação é UNICA</para>
    /// <para>Valor da parcela quando a transação é PARCELADA</para>
    /// </summary>
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
    /// <para>Número referente a parcela da transação</para>
    /// </summary>
    public int? ParcelaAtual { get; set; }

    /// <summary>
    /// <para>Obrigatório quando Parcelamento</para>
    /// <para>Quantidade que irá repetir ao cadastrar a transação num intervalo de meses</para>
    /// </summary>
    public int? NumeroParcelas { get; set; }


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
        ParcelaAtual = ParcelaAtual,
        NumeroParcelas = NumeroParcelas,
        ReferenciaParcelaId = ReferenciaParcelaId,
        Conta = Conta?.Descricao ?? "-----",
    };
}