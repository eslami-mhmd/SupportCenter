using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Users.CreateUser;
using SupportCenter.Domain.Users;

namespace SupportCenter.Api.Endpoints.Users;

public static class CreateUserEndpoint
{
    public static void MapCreateUser(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/users",
            async (
                CreateUserRequest request,
                IDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var id =
                    await dispatcher.Send<Guid>(
                        new CreateUserCommand(
                            request.OrganizationId,
                            request.Email,
                            request.DisplayName,
                            request.Role),
                        cancellationToken);


                return Results.Ok(
                    new
                    {
                        id
                    });
            });
    }
}


public sealed record CreateUserRequest(
    Guid OrganizationId,
    string Email,
    string DisplayName,
    UserRole Role);