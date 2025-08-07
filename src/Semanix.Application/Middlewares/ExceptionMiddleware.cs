using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Semanix.Application.Interfaces.Repositories;
using Semanix.Domain.AuditTrail;
using ApplicationException = Semanix.Common.CustomException.ApplicationException;

namespace Semanix.Application.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;
    private static JsonSerializerSettings? _serializerSettings;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
        _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ApplicationException ex)
        {
            _logger.LogError("Something went wrong: {Ex}", ex);
            await HandleExceptionAsync(httpContext, ex);
            //await _errorLogger.AddError(new ErrorLogger(ex, httpContext));
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong: {Ex}", ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, ApplicationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)exception.StatusCode;
        var message = exception.Message;
        await context.Response.WriteAsync(JsonConvert.SerializeObject(new Response.Response()
        {
            StatusCode = (int)exception.StatusCode,
            Message = message,
        }, _serializerSettings));
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;

        var message = exception switch
        {
            BadHttpRequestException => "Invalid request payload supplied",
            ApplicationException => exception.Message,
            UnauthorizedAccessException => "User does not have required permission to access this endpoint",
            _ => exception.Message ?? "Error occured while performing request, please try again after some time"
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(new Response.Response()
        {
            StatusCode = context.Response.StatusCode,
            Message = message
        }, _serializerSettings));
    }
}