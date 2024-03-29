﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using MyCoreBanking.API.Helpers;
using System.Text;
using System.Text.Json;

namespace MyCoreBanking.API.Extensions;

internal static class HttpRequestExtensions
{
    #region [+ CreateResult]

    // 4xx
    public static IActionResult CreateResult<TException>(this HttpRequest httpRequest, TException exception, ILogger logger)
        where TException : Exception
    {
        logger.LogError(exception, $"log: {exception.Message}");

        return exception switch
        {
            ArgumentException
            or
            ArgumentNullException
            or
            IndexOutOfRangeException
            or
            InvalidOperationException => new ContentResult()
            {
                Content = exception.Message,
                ContentType = "plain/text",
                StatusCode = StatusCodes.Status400BadRequest,
            },
            ValidationException validationException => new ContentResult()
            {
                Content = validationException.Errors.First().ErrorMessage,
                ContentType = "plain/text",
                StatusCode = StatusCodes.Status400BadRequest,
            },
            UnauthorizedAccessException => new ContentResult()
            {
                Content = exception.Message,
                ContentType = "plain/text",
                StatusCode = StatusCodes.Status401Unauthorized,
            },
            SecurityTokenSignatureKeyNotFoundException => new ContentResult()
            {
                Content = "Token inválido, faça o login novamente.",
                ContentType = "plain/text",
                StatusCode = StatusCodes.Status401Unauthorized,
            },
            SecurityTokenExpiredException => new ContentResult()
            {
                Content = "Token expirado, faça o login novamente.",
                ContentType = "plain/text",
                StatusCode = StatusCodes.Status401Unauthorized,
            },
            NotFoundException => new ContentResult()
            {
                Content = exception.Message,
                ContentType = "plain/text",
                StatusCode = StatusCodes.Status404NotFound,
            },
            JsonException => new ContentResult()
            {
                Content = "O corpo da requisição não pode ser vazio ou não está no formato correto",
                ContentType = "plain/text",
                StatusCode = StatusCodes.Status400BadRequest,
            },
            _ => new ContentResult()
            {
                Content = "Ocorreu um erro inesperado",
                ContentType = "plain/text",
                StatusCode = StatusCodes.Status500InternalServerError,
            }
        };
    }

    // 200
    public static IActionResult CreateResult<TResult>(this HttpRequest httpRequest, TResult result)
        => new ContentResult()
        {
            Content = JsonSerializer.Serialize(result, ProjectConstants.JsonOptions),
            ContentType = "application/json",
            StatusCode = StatusCodes.Status200OK,
        };

    // 204
    public static IActionResult CreateResult(this HttpRequest httpRequest) => new NoContentResult();

    #endregion

    // Obter usuário logado
    public static Guid Authorize(this HttpRequest httpRequest)
    {
        var auth = httpRequest.Headers["Authorization"];

        if (auth == StringValues.Empty)
            throw new UnauthorizedAccessException("Você deve estar logado para acessar este recurso");

        var appSettingsService = httpRequest.HttpContext.RequestServices.GetRequiredService<AppSettings>();

        return JwtHelper.ValidateAndThrow(appSettingsService.JwtSecret, auth.ToString().Split(" ")[1]);
    }

    // Le o body do request json e deserializa para o tipo T
    public static async Task<T> ReadJsonBodyAsAsync<T>(this HttpRequest httpRequest)
    {
        var body = httpRequest.Body;

        using var reader = new StreamReader(body, Encoding.UTF8);

        var json = await reader.ReadToEndAsync();

        return JsonSerializer.Deserialize<T>(json, ProjectConstants.JsonOptions)
            ?? throw new InvalidOperationException("O corpo da requisição não pode ser vazio");
    }
}
