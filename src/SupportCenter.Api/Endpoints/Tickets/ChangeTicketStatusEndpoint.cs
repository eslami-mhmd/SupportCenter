using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Tickets.ChangeTicketStatus;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Api.Endpoints.Tickets;

public static class ChangeTicketStatusEndpoint
{
    public static void MapChangeTicketStatus(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/tickets/{id:guid}/status",
            async (
                Guid id,
                ChangeTicketStatusRequest request,
                IDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var ticketId =
                    await dispatcher.Send<Guid>(
                        new ChangeTicketStatusCommand(
                            id,
                            request.Status),
                        cancellationToken);


                return Results.Ok(
                    new
                    {
                        id = ticketId
                    });
            });
    }
}


public sealed record ChangeTicketStatusRequest(
    TicketStatus Status);