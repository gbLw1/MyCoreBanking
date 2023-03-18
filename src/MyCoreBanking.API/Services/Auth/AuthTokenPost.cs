using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking.API;
using MyCoreBanking.API.Data;
using MyCoreBanking.Args;

namespace FreedomHub.API.Services.Auth;

public static class AuthTokenPost
{
    [FunctionName(nameof(AuthTokenPost))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "auth/token")] HttpRequest httpRequest,
        ILogger logger)
    {
        try
        {
            //if (!httpRequest.HasFormContentType)
            //    throw new InvalidOperationException(message: "Parameters needed");

            //var formCollection = await httpRequest.ReadFormAsync();

            //if (!formCollection.TryGetValue("email", out StringValues usernameStringValue))
            //    throw new ArgumentException();

            //if (!formCollection.TryGetValue("password", out StringValues passwordStringValue))
            //    throw new ArgumentException();

            //var email = usernameStringValue.ToString();
            //var password = passwordStringValue.ToString();

            var _context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<AuthTokenPostArgs>();

            new AuthTokenPostArgs.Validator().ValidateAndThrow(args);

            var usuarioEntity = await _context
                .Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(lbda => lbda.Email == args.Email);

            if (usuarioEntity is null)
                throw new UnauthorizedAccessException("Usuário ou senha incorretos");

            if (!usuarioEntity.SenhaValida(args.Senha))
            {
                throw new UnauthorizedAccessException("Usuário ou senha incorretos");
            }

            var appSettings = httpRequest.HttpContext.RequestServices.GetRequiredService<AppSettings>();

            // LMAO
            var expiracaoEmMinutos = 60 * 24 * 30;

            return httpRequest.CreateResult(
                new
                {
                    tokenType = "Bearer",
                    accessToken = JwtExtension.GenerateJwtToken(appSettings.JwtSecret, httpRequest.Host.Host, httpRequest.Host.Host, expiracaoEmMinutos, usuarioEntity.Id.ToString()),
                    expiresIn = DateTime.Now.AddMinutes(expiracaoEmMinutos),
                });
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}