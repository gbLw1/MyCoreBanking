using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCoreBanking.API.Data;
using MyCoreBanking.API.Data.Entities;
using MyCoreBanking.Args;

namespace MyCoreBanking.API.Services.Usuarios;

public class UsuariosPost
{
    [FunctionName(nameof(UsuariosPost))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "usuarios")] HttpRequest httpRequest,
        ILogger logger)
    {
        var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

        var content = await httpRequest.ReadAsStringAsync();

        var args = JsonSerializer.Deserialize<UsuariosPostArgs>(content)!;

        var usuarioEntity = new Usuario
        {
            Nome = args.Nome,
            Email = args.Email,
        };

        usuarioEntity.HashSenha(args.Senha);

        context.Usuarios.Add(usuarioEntity);
        await context.SaveChangesAsync();

        return new OkResult();
    }
}
