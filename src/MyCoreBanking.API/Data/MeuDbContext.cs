using Microsoft.EntityFrameworkCore;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data;

internal class MeuDbContext : DbContext
{
    public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options)
    { }

    public DbSet<Transacao> Transacoes { get; set; } = default!;
    public DbSet<Usuario> Usuarios { get; set; } = default!;
    public DbSet<MeioDePagamento> MeiosDePagamento { get; set; } = default!;
    public DbSet<MeioDePagamentoContaCorrente> ContasCorrente { get; set; } = default!;
    public DbSet<MeioDePagamentoCartaoDeCredito> CartoesDeCredito { get; set; } = default!;
}
