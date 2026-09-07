using System.Net;
using System.Text.Json;
using SupportCenter.Application.Exceptions;

namespace SupportCenter.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;


    public ExceptionHandlingMiddleware(
        RequestDelegate next)
    {
        _next = next;
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

    private static async Task HandleExceptionAsync(
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

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(
            new
            {
                error = "An unexpected error occurred."
            });
    }
}