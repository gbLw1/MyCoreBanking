namespace MyCoreBanking.Models;

public class CartaoDeCreditoModel : MeioDePagamentoModel
{
    public string NumerosFinais { get; set; } = string.Empty;
    public Banco? Banco { get; set; }
    public BandeiraCartao Bandeira { get; set; }
}
