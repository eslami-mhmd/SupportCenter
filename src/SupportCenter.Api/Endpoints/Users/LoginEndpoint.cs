using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Auth.Login;
using SupportCenter.Application.Features.Auth.Logout;

namespace SupportCenter.Api.Endpoints.Users;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (
            LoginRequest request,
            IDispatcher dispatcher,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginCommand(
                request.Email,
                request.Password);

            var userId = await dispatcher.Send<Guid>(
                command,
                cancellationToken);

            return Results.Ok(new LoginResponse(userId));
        });

        app.MapPost("/api/auth/logout", async (
            IDispatcher dispatcher,
            CancellationToken cancellationToken) =>
        {
            await dispatcher.Send<bool>(
                new LogoutCommand(),
                cancellationToken);

            return Results.Ok();
        });

        return app;
    }
}

public sealed record LoginRequest(
    string Email,
    string Password);

public sealed record LoginResponse(Guid Id);