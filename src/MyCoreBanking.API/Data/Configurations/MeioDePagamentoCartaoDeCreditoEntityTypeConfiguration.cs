using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // public string Bandeira { get; set; } = string.Empty;
        builder.Property(m => m.Bandeira)
            .IsRequired()
            .HasMaxLength(50);

        // public MeioDePagamento? MeioDePagamento { get; set; }
        builder.HasOne(m => m.MeioDePagamento)
            .WithOne(m => m.CartaoDeCredito)
            .HasForeignKey<MeioDePagamentoCartaoDeCredito>(m => m.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}