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

        // public decimal Valor { get; set; }
        builder
            .Property(x => x.Valor)
            .HasPrecision(18, 2)
            .IsRequired();

        // public DateTime? DataEfetivacao { get; set; }
        builder
            .Property(x => x.DataEfetivacao)
            .IsRequired(false);

        // public DateTime DataTransacao { get; set; }
        builder
            .Property(x => x.DataTransacao)
            .IsRequired();

        // public OperacaoTipo TipoOperacao { get; set; }
        builder
            .Property(x => x.TipoOperacao)
            .HasConversion<string>()
            .IsRequired();

        // public TransacaoTipo TipoTransacao { get; set; }
        builder
            .Property(x => x.TipoTransacao)
            .HasConversion<string>()
            .IsRequired();

        // public MeioDePagamentoTipo MeioPagamento { get; set; }
        builder
            .Property(x => x.MeioPagamento)
            .HasConversion<string>()
            .IsRequired();

        // public Categoria Categoria { get; set; }
        builder
            .Property(x => x.Categoria)
            .HasConversion<string>()
            .IsRequired();

        // public Guid? ReferenciaParcelaId { get; set; }
        builder
            .Property(x => x.ReferenciaParcelaId)
            .IsRequired(false);

        // public int? NumeroParcelas { get; set; }
        builder
            .Property(x => x.NumeroParcelas)
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
