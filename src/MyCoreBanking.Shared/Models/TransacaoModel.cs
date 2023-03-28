namespace MyCoreBanking.Models;

public class TransacaoModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataPagamento { get; set; }
    public DateTime? DataTransacao { get; set; }
    public OperacaoTipo TipoDeOperacao { get; set; }
    public TransacaoTipo TipoDeTransacao { get; set; }
    public MeioDePagamentoTipo MeioDePagamento { get; set; }
    public Categoria Categoria { get; set; }
    public DateTime? DataVencimento { get; set; }
    public int? NumeroParcelas { get; set; }
    public decimal? ValorParcela { get; set; }
    public Guid? ReferenciaParcelaId { get; set; }
}