using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Sla.CreateSlaPolicy;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Api.Endpoints.Sla;

public static class CreateSlaPolicyEndpoint
{
    public static void MapCreateSlaPolicy(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/sla-policies",
            async (
                CreateSlaPolicyRequest request,
                IDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var id =
                    await dispatcher.Send<Guid>(
                        new CreateSlaPolicyCommand(
                            request.OrganizationId,
                            request.Priority,
                            request.ResponseTimeMinutes,
                            request.ResolutionTimeMinutes),
                        cancellationToken);

                return Results.Ok(
                    new
                    {
                        id
                    });
            });
    }
}

public sealed record CreateSlaPolicyRequest(
    Guid OrganizationId,
    TicketPriority Priority,
    int ResponseTimeMinutes,
    int ResolutionTimeMinutes);