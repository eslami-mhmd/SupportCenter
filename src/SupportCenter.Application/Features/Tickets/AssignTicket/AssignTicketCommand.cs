using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Tickets.AssignTicket;

public sealed record AssignTicketCommand(
    Guid TicketId,
    Guid UserId)
    : ICommand<bool>;