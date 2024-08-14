using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyCoreBanking.API.Data;

internal sealed class MeuDbContextFactory : IDesignTimeDbContextFactory<MeuDbContext>
{
    public MeuDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MeuDbContext>();

        var strBuilder = new StringBuilder();
        strBuilder.Append("Server=localhost;");
        strBuilder.Append("Initial Catalog=MyCoreBanking;");
        strBuilder.Append("Persist Security Info=False;");
        strBuilder.Append("User ID=sa;");
        strBuilder.Append("Password=ALGUMA_SENHA_MUITO_FORTE_COM_CHAR_ESPECIAL_12345_!@#;");
        strBuilder.Append("MultipleActiveResultSets=False;");
        strBuilder.Append("Encrypt=True;");
        strBuilder.Append("TrustServerCertificate=True;");
        strBuilder.Append("Connection Timeout=30;");

        var connectionString = strBuilder.ToString();

        optionsBuilder.UseSqlServer(
            connectionString: connectionString
            ?? throw new ArgumentException(connectionString));

        return new MeuDbContext(optionsBuilder.Options);
    }
}
