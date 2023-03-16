using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal sealed class MeioDePagamentoEntityTypeConfiguration : IEntityTypeConfiguration<MeioDePagamento>
{
    public void Configure(EntityTypeBuilder<MeioDePagamento> builder)
    {
        builder.ToTable(nameof(MeioDePagamento));

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        // public string Apelido { get; set; } = string.Empty;
        builder.Property(m => m.Apelido)
            .IsRequired()
            .HasMaxLength(50);

        // public string? Observacao { get; set; }
        builder.Property(m => m.Observacao)
            .HasMaxLength(250);

        // public MeioDePagamentoTipo Tipo { get; set; }
        builder.Property(m => m.Tipo)
            .IsRequired();

        // public Usuario? Usuario { get; set; }
        // public Guid UsuarioId { get; set; }
        builder.HasOne(m => m.Usuario)
            .WithMany(u => u.MeiosDePagamento)
            .HasForeignKey(m => m.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // public ICollection<Transacao>? Transacoes { get; set; }

        // public MeioDePagamentoCartaoDeCredito? CartaoDeCredito { get; set; }
        // public MeioDePagamentoContaCorrente? ContaCorrente { get; set; }
    }
}