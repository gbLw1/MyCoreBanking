namespace MyCoreBanking.API.Data.Entities;

internal sealed class Cartao : BaseEntity
{
    public string Numero { get; set; } = default!;

    public string DataValidade { get; set; } = default!;


    public ContaBancaria Conta { get; set; } = new();
}