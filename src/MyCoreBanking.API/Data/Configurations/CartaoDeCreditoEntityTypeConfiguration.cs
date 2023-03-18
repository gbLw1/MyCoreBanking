using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal sealed class CartaoDeCreditoEntityTypeConfiguration : IEntityTypeConfiguration<CartaoDeCredito>
{
    public void Configure(EntityTypeBuilder<CartaoDeCredito> builder)
    {
        builder.ToTable(nameof(CartaoDeCredito));

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

        //public MeioDePagamento MeioDePagamento { get; set; } = null!;
        builder.HasOne(m => m.MeioDePagamento)
                .WithOne(m => m.CartaoDeCredito)
                .HasForeignKey<CartaoDeCredito>(m => m.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        builder.Navigation(m => m.MeioDePagamento)
                .IsRequired()
                .AutoInclude();
    }
}