namespace MyCoreBanking.API.Data.Entities;

internal sealed class Usuario : BaseDataEntity
{
    public string Nome { get; set; } = default!;

    // public string Cpf { get; set; } = default!; realmente ncessário?

    public string Email { get; set; } = default!;

    // public DateTime Nascimento { get; set; } realmente necessário?

    public string Senha { get; set; } = default!;

    public ICollection<Transacao>? Transacoes { get; set; }
    public ICollection<MeioDePagamento>? MeiosDePagamento { get; set; }

    public ICollection<MeioDePagamentoContaCorrente>? ContaCorrentes { get; set; }
    public ICollection<MeioDePagamentoCartaoDeCredito>? CartaoDeCreditos { get; set; }
}