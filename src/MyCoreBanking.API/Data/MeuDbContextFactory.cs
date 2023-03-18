using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyCoreBanking.API.Data;

internal sealed class MeuDbContextFactory : IDesignTimeDbContextFactory<MeuDbContext>
{
    public MeuDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MeuDbContext>();
        optionsBuilder.UseSqlServer(
            connectionString: "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=MyCoreBanking; Trusted_Connection=True; TrustServerCertificate=True");

        return new MeuDbContext(optionsBuilder.Options);
    }
}