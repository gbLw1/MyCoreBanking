namespace MyCoreBanking.Models;

public class ContaModel
{
    public Guid Id { get; set; }
    public decimal Saldo { get; set; }
    public Banco Banco { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public ContaTipo Tipo { get; set; }
}