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
    public DbSet<ContaCorrente> ContasCorrente { get; set; } = default!;
    public DbSet<CartaoDeCredito> CartoesDeCredito { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
