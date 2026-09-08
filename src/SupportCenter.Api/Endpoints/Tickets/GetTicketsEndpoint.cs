using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Tickets.ListTickets;
using SupportCenter.Application.Features.Tickets.GetTicket;
using SupportCenter.Application.Common.Pagination;

namespace SupportCenter.Api.Endpoints.Tickets;

public static class ListTicketsEndpoint
{
    public static void MapListTickets(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/tickets",
            async (
                Guid organizationId,
                string? status,
                int pageNumber,
                int pageSize,
                IDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var result = await dispatcher.Send<
                    PagedResult<TicketDto>>(
                    new ListTicketsQuery(
                        organizationId,
                        status,
                        pageNumber,
                        pageSize),
                    cancellationToken);


                return Results.Ok(result);
            });
    }
}