using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Tickets.UpdateTicket;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Api.Endpoints.Tickets;

public static class UpdateTicketEndpoint
{
    public static void MapUpdateTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/tickets/{id}",
            async (
                Guid id,
                UpdateTicketRequest request,
                IDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var result = await dispatcher.Send<Guid>(
                    new UpdateTicketCommand(
                        id,
                        request.Title,
                        request.Description,
                        request.Priority),
                    cancellationToken);

                return Results.Ok(result);
            })
            .WithTags("Tickets");
    }
}

public sealed record UpdateTicketRequest(
    string? Title,
    string? Description,
    TicketPriority? Priority);
