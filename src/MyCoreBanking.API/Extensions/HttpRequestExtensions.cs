using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MyCoreBanking.API.Extensions;

internal static class HttpRequestExtensions
{
    // 4xx
    public static IActionResult CreateResult<TException>(this HttpRequest req, TException exception, ILogger logger)
        where TException : Exception
    => exception switch
    {
        UnauthorizedAccessException => new UnauthorizedObjectResult(exception.Message),
        _ => new BadRequestObjectResult(exception.Message),
    };

    // 200
    public static IActionResult CreateResult<TResult>(this HttpRequest req, TResult result)
    {
        return new OkObjectResult(result);
    }

    // 204
    public static IActionResult CreateResult(this HttpRequest req) => new OkResult();

    public static Guid Authorize(this HttpRequest req)
    {
        var auth = req.Headers["Authorization"];

        var appSettingsService = req.HttpContext.RequestServices.GetRequiredService<AppSettings>();

        return JwtExtension.ValidateAndThrow(appSettingsService.JwtSecret, auth.ToString().Split(" ")[1]);
    }
}
