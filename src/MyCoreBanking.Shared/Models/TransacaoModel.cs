namespace MyCoreBanking.Models;

public class TransacaoModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = default!;
    public string? Observacao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
    public Guid MeioDePagamentoId { get; set; }
    // public MeioDePagamentoModel? MeioDePagamento { get; set; }
}
