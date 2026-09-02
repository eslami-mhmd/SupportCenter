using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Organizations.CreateOrganization;

namespace SupportCenter.Api.Endpoints.Organizations;

public static class CreateOrganizationEndpoint
{
    public static void MapCreateOrganization(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/organizations",
            async (
                CreateOrganizationRequest request,
                IDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var command =
                    new CreateOrganizationCommand(
                        request.Name,
                        request.Slug);


                var id =
                    await dispatcher.Send(
                        command,
                        cancellationToken);


                return Results.Created(
                    $"/api/organizations/{id}",
                    new
                    {
                        Id = id
                    });
            });
    }
}


public sealed record CreateOrganizationRequest(
    string Name,
    string Slug);