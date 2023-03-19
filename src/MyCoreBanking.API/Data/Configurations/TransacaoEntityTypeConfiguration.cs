using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal sealed class TransacaoEntityTypeConfiguration : BaseEntityTypeConfiguration<TransacaoEntity>
{
    public override void Configure(EntityTypeBuilder<TransacaoEntity> builder)
    {
        base.Configure(builder);

        // public string Descricao { get; set; } = default!;
        builder.Property(t => t.Descricao)
            .IsRequired()
            .HasMaxLength(250);

        // public string? Observacao { get; set; }
        builder.Property(t => t.Observacao)
            .HasMaxLength(250);

        // public decimal Valor { get; set; }
        builder.Property(t => t.Valor)
            .IsRequired();

        // public DateTime? DataPagamento { get; set; }
        builder.Property(t => t.DataPagamento)
            .IsRequired();

        // public Usuario? Usuario { get; set; }
        // public Guid UsuarioId { get; set; }
        builder.HasOne(t => t.Usuario)
            .WithMany(u => u.Transacoes)
            .HasForeignKey(t => t.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        // public MeioDePagamento? MeioDePagamento { get; set; }
        // public Guid MeioDePagamentoId { get; set; }
        builder.HasOne(t => t.MeioDePagamento)
            .WithMany(m => m.Transacoes)
            .HasForeignKey(t => t.MeioDePagamentoId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        // public TransacaoTipo Tipo { get; set; }
        builder.Property(t => t.Tipo)
            .HasConversion<string>()
            .IsRequired();
    }
}
