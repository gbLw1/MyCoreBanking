using Microsoft.EntityFrameworkCore;

namespace MyCoreBanking.API.Data;

internal class MeuDbContext : DbContext
{
    public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options)
    {
    }


}
