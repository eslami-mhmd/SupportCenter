using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Application.Features.Tickets.ChangeTicketStatus;

public sealed record ChangeTicketStatusCommand(
    Guid TicketId,
    TicketStatus Status
) : ICommand<Guid>;