using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal sealed class TransacaoEntityTypeConfiguration : BaseEntityTypeConfiguration<TransacaoEntity>
{
    public override void Configure(EntityTypeBuilder<TransacaoEntity> builder)
    {
        base.Configure(builder);

        // public string Descricao { get; set; } = string.Empty;
        builder
            .Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(100);

        // public string? Observacao { get; set; }
        builder
            .Property(x => x.Observacao)
            .IsRequired(false)
            .HasMaxLength(250);

        // public TransacaoTipo Tipo { get; set; }
        builder
            .Property(x => x.Tipo)
            .HasConversion<string>()
            .IsRequired();

        // public decimal Valor { get; set; }
        builder
            .Property(x => x.Valor)
            .HasPrecision(18, 2)
            .IsRequired();

        // public DateTime? DataPagamento { get; set; }
        builder
            .Property(x => x.DataPagamento)
            .IsRequired(false);

        // public MeioDePagamentoTipo MeioPagamento { get; set; }
        builder
            .Property(x => x.MeioDePagamento)
            .HasConversion<string>()
            .IsRequired();

        // public Categoria Categoria { get; set; }
        builder
            .Property(x => x.Categoria)
            .HasConversion<string>()
            .IsRequired();

        // public bool Recorrente { get; set; }
        builder
            .Property(x => x.Recorrente)
            .IsRequired();

        // public DateTime? DataVigenciaInicio { get; set; }
        builder
            .Property(x => x.DataVigenciaInicio)
            .IsRequired(false);

        // public DateTime? DataVigenciaFim { get; set; }
        builder
            .Property(x => x.DataVigenciaFim)
            .IsRequired(false);

        // public int? DiaVencimento { get; set; }
        builder
            .Property(x => x.DiaVencimento)
            .IsRequired(false);

        // public int? NumeroParcelas { get; set; }
        builder
            .Property(x => x.NumeroParcelas)
            .IsRequired(false);

        // public decimal? ValorParcela { get; set; }
        builder
            .Property(x => x.ValorParcela)
            .HasPrecision(18, 2)
            .IsRequired(false);

        // public Guid UsuarioId { get; set; }
        // public UsuarioEntity? Usuario { get; set; }
        builder
            .HasOne(x => x.Usuario)
            .WithMany(x => x.Transacoes)
            .HasForeignKey(x => x.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        // public Guid ContaId { get; set; }
        // public ContaEntity? Conta { get; set; }
        builder
            .HasOne(x => x.Conta)
            .WithMany(x => x.Transacoes)
            .HasForeignKey(x => x.ContaId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
