using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Tickets.CreateTicket;

namespace SupportCenter.Api.Endpoints.Tickets;

public static class CreateTicketEndpoint
{
    public static void MapCreateTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/tickets",
            async (
                CreateTicketRequest request,
                IDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var id = await dispatcher.Send<Guid>(
                    new CreateTicketCommand(
                        request.OrganizationId,
                        request.Title,
                        request.Description),
                    cancellationToken);

                return Results.Ok(id);
            });
    }
}


public sealed record CreateTicketRequest(
    Guid OrganizationId,
    string Title,
    string Description);