namespace MyCoreBanking.API.Data.Entities;

internal class MeioDePagamento : BaseDataEntity
{
    public string Apelido { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public MeioDePagamentoTipo Tipo { get; set; }

    public Usuario? Usuario { get; set; }
    public Guid UsuarioId { get; set; }

    public ICollection<Transacao>? Transacoes { get; set; }

    public CartaoDeCredito? CartaoDeCredito { get; set; }
    public ContaCorrente? ContaCorrente { get; set; }
}
