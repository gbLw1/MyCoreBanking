using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking.API.Data;

namespace MyCoreBanking.API.Services.Usuarios;

public class UsuariosGetById
{
    [FunctionName(nameof(UsuariosGetById))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "usuarios/{id:guid}")] HttpRequest httpRequest,
        ILogger logger,
        Guid id)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var usuario = await context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => u.ToModel())
                .FirstOrDefaultAsync();

            if (usuario is null)
                throw new NotFoundException(message: "Usuário não encontrado", paramName: nameof(id));

            return httpRequest.CreateResult(usuario);
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}
