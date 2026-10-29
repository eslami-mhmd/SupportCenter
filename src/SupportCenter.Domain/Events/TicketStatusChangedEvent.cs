using SupportCenter.Domain.Tickets;

namespace SupportCenter.Domain.Events;

public sealed class TicketStatusChangedEvent : DomainEvent
{
    public Guid TicketId { get; }
    public TicketStatus PreviousStatus { get; }
    public TicketStatus NewStatus { get; }

    public TicketStatusChangedEvent(
        Guid ticketId,
        TicketStatus previousStatus,
        TicketStatus newStatus)
    {
        TicketId = ticketId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
    }
}
