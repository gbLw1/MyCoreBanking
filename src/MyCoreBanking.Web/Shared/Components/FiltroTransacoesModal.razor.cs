namespace MyCoreBanking.Web.Shared.Components;

public class FiltroTransacoes
{
    public DateTime? DataEfetivacao { get; set; }

    public DateTime DataBusca { get; set; } = DateTime.Now;

    public MeioPagamentoTipo? MeioPagamento { get; set; }

    public OperacaoTipo? TipoOperacao { get; set; }

    public TransacaoTipo? TipoTransacao { get; set; }

    public Categoria? Categoria { get; set; }
}