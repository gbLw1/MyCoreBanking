using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal sealed class UsuarioEntityTypeConfiguration : BaseEntityTypeConfiguration<Usuario>
{
    public override void Configure(EntityTypeBuilder<Usuario> builder)
    {
        base.Configure(builder);

        // public string Nome { get; set; } = default!;
        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(250);

        // public string Email { get; set; } = default!;
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(250);

        // public string SenhaHash { get; private set; } = default!;
        builder.Property(u => u.SenhaHash)
            .IsRequired()
            .HasMaxLength(250);

        // Constraint: Email único
        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}
