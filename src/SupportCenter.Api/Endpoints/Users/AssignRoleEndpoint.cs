using Microsoft.AspNetCore.Mvc;
using SupportCenter.Application.Features.Users.AssignRole;
using SupportCenter.Application.Security;

namespace SupportCenter.Api.Endpoints.Users;

public static class AssignRoleEndpoint
{
    public static void MapAssignRoleEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/users/{userId:guid}/roles/{roleName}",
            async (
                Guid userId,
                string roleName,
                AssignUserRoleCommandHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.Handle(
                    new AssignUserRoleCommand(
                        userId,
                        roleName),
                    cancellationToken);


                return Results.NoContent();
            })
            .RequireAuthorization(Permissions.RolesManage);
    }
}