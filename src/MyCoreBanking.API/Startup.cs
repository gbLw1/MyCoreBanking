using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyCoreBanking.API.Data;

[assembly: FunctionsStartup(typeof(MyCoreBanking.API.Startup))]
namespace MyCoreBanking.API;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        builder.Services.AddDbContext<MeuDbContext>(options =>
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection")
                ?? throw new ArgumentException("ConnectionStrings:DefaultConnection"));

            options.EnableSensitiveDataLogging().LogTo(Console.WriteLine);
            options.EnableDetailedErrors().LogTo(Console.WriteLine);
        });

        var appSettings = new AppSettings
        {
            JwtSecret = Environment.GetEnvironmentVariable("JwtSecret")
                ?? throw new ArgumentException("JwtSecret"),
        };

        builder.Services.AddSingleton(appSettings);
    }
}