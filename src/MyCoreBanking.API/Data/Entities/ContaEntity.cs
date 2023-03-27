using MyCoreBanking.Models;

namespace MyCoreBanking.API.Data.Entities;

internal class ContaEntity : BaseDataEntity
{
    public decimal Saldo { get; set; }
    public Banco Banco { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public ContaTipo Tipo { get; set; }

    public Guid UsuarioId { get; set; }
    public UsuarioEntity Usuario { get; set; } = null!;

    public ICollection<TransacaoEntity>? Transacoes { get; set; }


    public ContaModel ToModel() => new()
    {
        Id = Id,
        Saldo = Saldo,
        Banco = Banco,
        Descricao = Descricao,
        Tipo = Tipo,
    };
}
