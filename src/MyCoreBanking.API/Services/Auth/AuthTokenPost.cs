using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using MyCoreBanking.API;
using MyCoreBanking.API.Data;
using MyCoreBanking.API.Helpers;

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
            if (!httpRequest.HasFormContentType)
                throw new InvalidOperationException(message: "Parameters needed");

            var formCollection = await httpRequest.ReadFormAsync();

            if (!formCollection.TryGetValue("email", out StringValues usernameStringValue))
                throw new ArgumentException();

            if (!formCollection.TryGetValue("password", out StringValues passwordStringValue))
                throw new ArgumentException();

            var email = usernameStringValue.ToString();
            var password = passwordStringValue.ToString();

            var _context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var usuarioEntity = await _context
                .Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(lbda => lbda.Email == email);

            if (usuarioEntity is null)
                throw new UnauthorizedAccessException();

            if (usuarioEntity.VerificarSenha(password))
            {
                throw new UnauthorizedAccessException();
            }

            var appSettings = httpRequest.HttpContext.RequestServices.GetRequiredService<AppSettings>();

            return new OkObjectResult(
                new
                {
                    tokenType = "Bearer",
                    accessToken = JwtHelper.GenerateJwtToken(appSettings.JwtSecret, httpRequest.Host.Host, httpRequest.Host.Host, 60, usuarioEntity.Id.ToString()),
                    expiresIn = DateTime.Now.AddHours(1),
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }
}