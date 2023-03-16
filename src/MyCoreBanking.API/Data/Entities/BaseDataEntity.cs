namespace MyCoreBanking.API.Data.Entities;

internal abstract class BaseDataEntity : BaseEntity
{
    public DateTime CriadoEm { get; set; } = DateTime.Now;

    public DateTime UltimaAtualizacaoEm { get; set; } = DateTime.Now;
}