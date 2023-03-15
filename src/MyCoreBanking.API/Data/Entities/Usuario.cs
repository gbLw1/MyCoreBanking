namespace MyCoreBanking.API.Data.Entities;

internal sealed class Usuario : BaseEntity
{
    public string Nome { get; set; } = default!;

    // public string Cpf { get; set; } = default!; realmente ncessário?

    public string Email { get; set; } = default!;

    // public DateTime Nascimento { get; set; } realmente necessário?

    public string Senha { get; set; } = default!;

    public IEnumerable<ContaBancaria>? ContasBancarias { get; set; }
    public IEnumerable<Cartao>? Cartoes { get; set; }
    public IEnumerable<Transacao>? Transacoes { get; set; }
}