using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal sealed class ContaEntityTypeConfiguration : BaseEntityTypeConfiguration<ContaEntity>
{
    public override void Configure(EntityTypeBuilder<ContaEntity> builder)
    {
        base.Configure(builder);

        // public decimal Saldo { get; set; }

        // public Banco Banco { get; set; }
        builder.Property(c => c.Banco)
                .IsRequired()
                .HasConversion<string>();

        // public string Descricao { get; set; } = string.Empty;
        builder.Property(c => c.Descricao)
                .IsRequired()
                .HasMaxLength(100);

        // public ContaTipo Tipo { get; set; }
        builder.Property(c => c.Tipo)
                .IsRequired()
                .HasConversion<string>();

        // public Guid UsuarioId { get; set; }
        // public UsuarioEntity Usuario { get; set; } = null!;
        builder.HasOne(c => c.Usuario)
                .WithMany(c => c.Contas)
                .HasForeignKey(c => c.UsuarioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
    }
}