using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal sealed class ContaCorrenteEntityTypeConfiguration : IEntityTypeConfiguration<ContaCorrente>
{
    public void Configure(EntityTypeBuilder<ContaCorrente> builder)
    {
        builder.ToTable(nameof(ContaCorrente));

        builder.HasKey(m => m.Id);

        //public Banco Banco { get; set; }
        builder.Property(m => m.Banco)
                .IsRequired()
                .HasConversion<string>();

        //public string Agencia { get; set; } = string.Empty;
        builder.Property(m => m.Agencia)
                .IsRequired()
                .HasMaxLength(4);

        //public string Conta { get; set; } = string.Empty;
        builder.Property(m => m.Conta)
                .IsRequired()
                .HasMaxLength(10);

        //public MeioDePagamento MeioDePagamento { get; set; } = null!;
        builder.HasOne(m => m.MeioDePagamento)
                .WithOne(m => m.ContaCorrente)
                .HasForeignKey<ContaCorrente>(m => m.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(m => m.MeioDePagamento)
                .IsRequired()
                .AutoInclude();
    }
}