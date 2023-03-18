using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal sealed class MeioDePagamentoEntityTypeConfiguration : BaseEntityTypeConfiguration<MeioDePagamento>
{
    public override void Configure(EntityTypeBuilder<MeioDePagamento> builder)
    {
        base.Configure(builder);

        // public string Apelido { get; set; } = string.Empty;
        builder.Property(m => m.Apelido)
            .IsRequired()
            .HasMaxLength(50);

        // public string? Observacao { get; set; }
        builder.Property(m => m.Observacao)
            .HasMaxLength(250);

        // public MeioDePagamentoTipo Tipo { get; set; }
        builder.Property(m => m.Tipo)
            .IsRequired()
            .HasConversion<string>();

        // public Usuario? Usuario { get; set; }
        // public Guid UsuarioId { get; set; }
        builder.HasOne(m => m.Usuario)
            .WithMany(u => u.MeiosDePagamento)
            .HasForeignKey(m => m.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        // public ICollection<Transacao>? Transacoes { get; set; }

        // public MeioDePagamentoCartaoDeCredito? CartaoDeCredito { get; set; }
        // public MeioDePagamentoContaCorrente? ContaCorrente { get; set; }
    }
}
