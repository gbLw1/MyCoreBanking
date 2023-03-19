namespace MyCoreBanking.Models;

public class ContaCorrenteModel : MeioDePagamentoModel
{
    public Banco Banco { get; set; }
    public string Agencia { get; set; } = string.Empty;
    public string Conta { get; set; } = string.Empty;
}
