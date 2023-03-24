using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking.API.Data;

namespace MyCoreBanking.API.Services.Usuarios;

public class UsuariosGet
{
    /// <summary>
    /// Endpoint teste para retornar todos os usuários com suas contas
    /// </summary>
    [FunctionName(nameof(UsuariosGet))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "usuarios")] HttpRequest httpRequest,
        ILogger logger)
    {
        try
        {
            var userId = httpRequest.Authorize();

            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            // verificar se o usuário que está acessando o endpoint é o adm
            // adm = 894437e0-77ca-ed11-995b-60dd8e7609d4
            if (userId != Guid.Parse("894437e0-77ca-ed11-995b-60dd8e7609d4"))
                throw new UnauthorizedAccessException("Usuário não autorizado");

            var usuarios = await context.Usuarios
                .Include(u => u.Contas)
                .ToListAsync();

            return httpRequest.CreateResult(
                usuarios.Select(u => new
                {
                    u.Id,
                    u.Nome,
                    u.Email,
                    Contas = u.Contas!.Select(c => new
                    {
                        c.Id,
                        c.Descricao,
                        c.Saldo,
                        c.Tipo,
                        c.Banco,
                    })
                })
            );
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}
