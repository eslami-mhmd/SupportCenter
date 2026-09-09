using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Tickets.AssignTicket;

namespace SupportCenter.Api.Endpoints.Tickets;

public static class AssignTicketEndpoint
{
    public static void MapAssignTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/tickets/{id:guid}/assign/{userId:guid}",
            async (
                Guid id,
                Guid userId,
                IDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await dispatcher.Send<bool>(
                        new AssignTicketCommand(
                            id,
                            userId),
                        cancellationToken);


                return result
                    ? Results.Ok()
                    : Results.NotFound();
            });
    }
}