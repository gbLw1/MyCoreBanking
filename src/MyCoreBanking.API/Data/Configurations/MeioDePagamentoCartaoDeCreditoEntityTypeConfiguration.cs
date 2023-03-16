using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal sealed class MeioDePagamentoCartaoDeCreditoEntityTypeConfiguration : IEntityTypeConfiguration<MeioDePagamentoCartaoDeCredito>
{
    public void Configure(EntityTypeBuilder<MeioDePagamentoCartaoDeCredito> builder)
    {
        builder.ToTable(nameof(MeioDePagamentoCartaoDeCredito));

        builder.HasKey(m => m.Id);

        // public string NumerosFinais { get; set; } = string.Empty;
        builder.Property(m => m.NumerosFinais)
            .IsRequired()
            .HasMaxLength(4);

        //public Banco? Banco { get; set; }
        builder.Property(m => m.Banco)
                .IsRequired()
                .HasConversion<string>();

        //public BandeiraCartao Bandeira { get; set; }
        builder.Property(m => m.Bandeira)
                .IsRequired()
                .HasConversion<string>();

        // public MeioDePagamento? MeioDePagamento { get; set; }
        builder.HasOne(m => m.MeioDePagamento)
            .WithOne(m => m.CartaoDeCredito)
            .HasForeignKey<MeioDePagamentoCartaoDeCredito>(m => m.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(m => m.MeioDePagamento)
            .IsRequired()
            .AutoInclude();
    }
}