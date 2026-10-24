using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Users.CreateUser;

namespace SupportCenter.Api.Endpoints.Users;

public static class CreateUserEndpoint
{
    public static IEndpointRouteBuilder MapCreateUserEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", async (
            CreateUserRequest request,
            IDispatcher dispatcher,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateUserCommand(
                request.Email,
                request.Password,
                request.OrganizationId);

            var userId = await dispatcher.Send<Guid>(
                command,
                cancellationToken);

            return Results.Created(
                $"/api/users/{userId}",
                new CreateUserResponse(userId));
        });

        return app;
    }
}

public sealed record CreateUserRequest(
    string Email,
    string Password,
    Guid? OrganizationId);

public sealed record CreateUserResponse(Guid Id);