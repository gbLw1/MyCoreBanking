using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(typeof(T).Name);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        if (typeof(T).IsAssignableTo(typeof(BaseDataEntity)))
        {
            builder
                .Property<DateTime>(nameof(BaseDataEntity.CriadoEm))
                .IsRequired();

            builder
                .Property<DateTime>(nameof(BaseDataEntity.UltimaAtualizacaoEm))
                .IsRequired();
        }
    }
}
