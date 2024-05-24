using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace receptai.api;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        /* Log info about called function & pass to next middleware */
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "<null>";
        logger.LogInformation($"Endpoint called: {context.Request.Method} {context.Request.Path} by {ip}");
        await next(context);
    }

}
