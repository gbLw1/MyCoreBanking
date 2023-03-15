using Microsoft.EntityFrameworkCore;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data;

internal class MeuDbContext : DbContext
{
    public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options)
    {
    }

    public DbSet<ContaBancaria> ContasBancarias { get; set; } = default!;
    public DbSet<Transacao> Transacoes { get; set; } = default!;
    public DbSet<Usuario> Usuarios { get; set; } = default!;
    public DbSet<Cartao> Cartoes { get; set; } = default!;
}
