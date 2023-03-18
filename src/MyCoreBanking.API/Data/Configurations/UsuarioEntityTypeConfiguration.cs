using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

public class UsuarioEntityTypeConfiguration : IEntityTypeConfiguration<Usuario>
{
    void IEntityTypeConfiguration<Usuario>.Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable(nameof(Usuario));

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(t => t.CriadoEm)
            .IsRequired();

        builder.Property(t => t.UltimaAtualizacaoEm)
            .IsRequired();


        // public string Nome { get; set; } = default!;
        builder.Property(t => t.Nome)
            .IsRequired()
            .HasMaxLength(250);

        // public string Email { get; set; } = default!;
        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(250);

        // public string SenhaHash { get; private set; } = default!;
        builder.Property(t => t.SenhaHash)
            .IsRequired()
            .HasMaxLength(250);
    }
}
