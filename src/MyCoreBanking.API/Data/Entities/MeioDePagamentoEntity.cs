using MyCoreBanking.Models;

namespace MyCoreBanking.API.Data.Entities;

internal class MeioDePagamentoEntity : BaseDataEntity
{
    public string Apelido { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public MeioDePagamentoTipo Tipo { get; set; }

    public UsuarioEntity? Usuario { get; set; }
    public Guid UsuarioId { get; set; }

    public ICollection<TransacaoEntity>? Transacoes { get; set; }

    public CartaoDeCreditoEntity? CartaoDeCredito { get; set; }
    public ContaCorrenteEntity? ContaCorrente { get; set; }


    public MeioDePagamentoModel ToModel() => new()
    {
        Id = Id,
        Apelido = Apelido,
        Observacao = Observacao,
        Tipo = Tipo,
    };
}
