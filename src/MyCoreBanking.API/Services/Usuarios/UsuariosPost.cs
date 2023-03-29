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

            // o usuário deve iniciar com uma conta pré cadastrada
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var usuarioEntity = new UsuarioEntity
                {
                    Nome = args.Nome,
                    Email = args.Email,
                };

                usuarioEntity.HashSenha(args.Senha);

                await context.Usuarios.AddAsync(usuarioEntity);
                await context.SaveChangesAsync();

                var contaEntity = new ContaEntity
                {
                    Saldo = 0,
                    Banco = Banco.Outro,
                    Descricao = "Carteira",
                    Tipo = ContaTipo.Carteira,
                    UsuarioId = usuarioEntity.Id,
                };

                await context.Contas.AddAsync(contaEntity);
                await context.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return httpRequest.CreateResult();
        }
        catch (Exception ex)
        {
            return httpRequest.CreateResult(ex, logger);
        }
    }
}
