using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using MyCoreBanking.API.Data;
using MyCoreBanking.API.Data.Entities;

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

            // TODO: obter serviço de criptografia -> bcrypt.net
            // TODO: obter unitOfWork ou dbContext diretamente

            Usuario? usuarioEntity = null;

            if (!formCollection.TryGetValue("username", out StringValues usernameStringValue))
                throw new ArgumentException();

            if (!formCollection.TryGetValue("password", out StringValues passwordStringValue))
                throw new ArgumentException();

            var username = usernameStringValue.ToString();
            var password = passwordStringValue.ToString();

            // obter contexto injetado
            var _context = new MeuDbContext(null);

            usuarioEntity = await _context
                .Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(lbda => lbda.Email == username)
                ?? throw new UnauthorizedAccessException();

            // TODO: implementar serviço de criptografia de senha -> bcrypt.net
            // https://github.com/BcryptNet/bcrypt.net

            // verificar se a senha informada convertida para a criptografia utilizada não for igual a senha salva no banco
            // throw UnauthorizedAccessException

            // TODO: retornar token JWT
            return new OkObjectResult(
                new
                {
                    tokenType = "Bearer",
                    accessToken = "JWT . . .",
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