using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking.API.Data;
using MyCoreBanking.Args;

namespace MyCoreBanking.API.Services.Usuarios;

public class UsuariosPUT
{
    [FunctionName(nameof(UsuariosPUT))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "usuarios")] HttpRequest httpRequest,
        ILogger logger)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<UsuariosPutArgs>();

            new UsuariosPutArgs.Validator().ValidateAndThrow(args);

            var usuarioEntity = await context.Usuarios
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (usuarioEntity is null)
                throw new NotFoundException(message: "Usuário não encontrado", paramName: nameof(userId));

            usuarioEntity.Nome = args.Nome;

            context.Usuarios.Update(usuarioEntity);
            await context.SaveChangesAsync();

            return httpRequest.CreateResult(new { usuarioEntity.Id });
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}
