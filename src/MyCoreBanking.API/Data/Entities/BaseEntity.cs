namespace MyCoreBanking.API.Data.Entities;

internal abstract class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.Now;

    public DateTime UltimaAtualizacaoEm { get; set; } = DateTime.Now;
}