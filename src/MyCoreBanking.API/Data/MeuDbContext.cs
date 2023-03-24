using Microsoft.EntityFrameworkCore;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data;

internal class MeuDbContext : DbContext
{
    public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options)
    { }

    public DbSet<TransacaoEntity> Transacoes { get; set; } = default!;
    public DbSet<UsuarioEntity> Usuarios { get; set; } = default!;
    public DbSet<ContaEntity> Contas { get; set; } = default!;
    // public DbSet<CartaoDeCreditoEntity> CartoesDeCredito { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
