namespace MyCoreBanking.API.Data.Entities;

internal sealed class Cartao : BaseEntity
{
    public string Nome { get; set; } = default!;
    public string DigitosFinais { get; set; } = default!;
    public string DataValidade { get; set; } = default!;
    public string Bandeira { get; set; } = default!;
    public DateTime Validade { get; set; }


    public ContaBancaria Conta { get; set; } = new();
}