using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
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
        try
        {
            var context = httpRequest.HttpContext.RequestServices.GetRequiredService<MeuDbContext>();

            var args = await httpRequest.ReadJsonBodyAsAsync<UsuariosPostArgs>();

            new UsuariosPostArgs.Validator().ValidateAndThrow(args);

            if (await context.Usuarios.AnyAsync(x => x.Email == args.Email))
                throw new InvalidOperationException("Email já cadastrado");

            var usuarioEntity = new UsuarioEntity
            {
                Nome = args.Nome,
                Email = args.Email,
            };

            usuarioEntity.HashSenha(args.Senha);

            // TODO: o usuário deve iniciar com uma conta corrente pré cadastrada

            context.Usuarios.Add(usuarioEntity);
            await context.SaveChangesAsync();

            return httpRequest.CreateResult();
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}
