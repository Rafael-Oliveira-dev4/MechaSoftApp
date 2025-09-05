using MechaSoft.Application.Common.Exceptions;
using MechaSoft.Application.Common.Responses;
using System.Net;
using System.Text.Json;

namespace MechaSoft.WebAPI.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (exception)
        {
            case ApplicationValidationException validationException:
                response = new ErrorResponse
                {
                    Code = "VALIDATION_ERROR",
                    Description = "One or more validation failures have occurred",
                    Details = validationException.Errors
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case MechaSoft.Application.Common.Exceptions.ApplicationException applicationException:
                response = new ErrorResponse
                {
                    Code = "APPLICATION_ERROR",
                    Description = applicationException.Message
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            default:
                response = new ErrorResponse
                {
                    Code = "INTERNAL_SERVER_ERROR",
                    Description = "An internal server error occurred"
                };
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}

public class ErrorResponse
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public object? Details { get; set; }
}