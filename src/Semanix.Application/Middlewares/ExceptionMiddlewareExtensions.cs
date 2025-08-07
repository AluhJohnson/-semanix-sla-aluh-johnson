using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Semanix.Application.Response;

namespace Semanix.Application.Middlewares;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    logger.LogError("Something went wrong: {ContextFeatureError}", contextFeature.Error);
                    await context.Response.WriteAsync(new Response.Response
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "Internal Server Error."
                    }.ToString() ?? string.Empty);
                }
            });
        });
    }

    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}