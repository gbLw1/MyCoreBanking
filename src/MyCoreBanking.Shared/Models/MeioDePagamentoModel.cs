namespace MyCoreBanking.Models;

public class MeioDePagamentoModel
{
    public Guid Id { get; set; }
    public string Apelido { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public MeioPagamentoTipo Tipo { get; set; }
}