namespace MyCoreBanking.Models;

public class TransacaoModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = default!;
    public string? Observacao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
    public Guid MeioDePagamentoId { get; set; } // permite a navegação para detalhes do meio de pagamento
    public string MeioDePagamentoApelido { get; set; } = string.Empty; // permite exibir o cartão de crédito ou conta bancária
    public MeioDePagamentoTipo MeioDePagamentoTipo { get; set; } // enum para facilitar a exibição do ícone e link para detalhes
}
