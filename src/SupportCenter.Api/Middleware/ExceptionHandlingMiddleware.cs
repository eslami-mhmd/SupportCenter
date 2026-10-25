using System.Net;
using System.Text.Json;
using SupportCenter.Application.Exceptions;

namespace SupportCenter.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;


    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context,
                ex);
        }
    }

    private async Task HandleExceptionAsync(
    HttpContext context,
    Exception exception)
    {
        context.Response.ContentType =
            "application/json";

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(
                new
                {
                    error = validationException.Message,
                    errors = validationException.Errors
                });

            return;
        }

        if (exception is UnauthorizedException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(
                new
                {
                    error = exception.Message
                });

            return;
        }

        _logger.LogError(
            exception,
            "Unhandled exception while processing request {Method} {Path}.",
            context.Request.Method,
            context.Request.Path);

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(
            new
            {
                error = "An unexpected error occurred."
            });
    }
}