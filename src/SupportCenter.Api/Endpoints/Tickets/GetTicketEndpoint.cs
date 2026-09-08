using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Tickets.GetTicket;

namespace SupportCenter.Api.Endpoints.Tickets;

public static class GetTicketEndpoint
{
    public static void MapGetTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/tickets/{id:guid}",
            async (
                Guid id,
                IDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var ticket = await dispatcher.Send<TicketDto?>(
                    new GetTicketQuery(id),
                    cancellationToken);


                return ticket is null
                    ? Results.NotFound()
                    : Results.Ok(ticket);
            });
    }
}