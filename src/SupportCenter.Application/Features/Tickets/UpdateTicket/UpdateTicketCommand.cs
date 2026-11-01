using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Application.Features.Tickets.UpdateTicket;

public sealed record UpdateTicketCommand(
    Guid TicketId,
    string? Title,
    string? Description,
    TicketPriority? Priority)
    : ICommand<Guid>;
