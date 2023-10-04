using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyCoreBanking.API.Data;

internal sealed class MeuDbContextFactory : IDesignTimeDbContextFactory<MeuDbContext>
{
    public MeuDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MeuDbContext>();

        optionsBuilder.UseSqlServer(
            connectionString: Environment.GetEnvironmentVariable(
                variable: "ConnectionStrings:DefaultConnection",
                target: EnvironmentVariableTarget.Process)
            ?? throw new ArgumentException("ConnectionStrings:DefaultConnection"));

        return new MeuDbContext(optionsBuilder.Options);
    }
}