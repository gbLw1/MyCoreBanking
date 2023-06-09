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
            options.UseSqlServer(Environment.GetEnvironmentVariable(
                variable: "ConnectionStrings:DefaultConnection",
                target: EnvironmentVariableTarget.Process)
                ?? throw new ArgumentException("ConnectionStrings:DefaultConnection"));

            // options.EnableSensitiveDataLogging().LogTo(Console.WriteLine);
            // options.EnableDetailedErrors().LogTo(Console.WriteLine);
        });

        builder.Services.AddSingleton(new AppSettings
        {
            JwtSecret = Environment.GetEnvironmentVariable(
                variable: "JwtSecret",
                target: EnvironmentVariableTarget.Process)
                ?? "1234567Teste@123",
        });
    }
}