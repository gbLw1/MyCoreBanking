namespace MyCoreBanking.Models;

public class TransacaoModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = default!;
    public string? Observacao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }

    // 1. permite a navegação para detalhes do meio de pagamento
    // 2. permite exibir o cartão de crédito ou conta bancária
    // 3. enum para facilitar a exibição do ícone e link para detalhes
    public MeioDePagamentoModel MeioDePagamento { get; set; } = null!;
}
