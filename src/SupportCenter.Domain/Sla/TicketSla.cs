using SupportCenter.Domain.Exceptions;

namespace SupportCenter.Domain.Sla;

public sealed class TicketSla
{
    private TicketSla()
    {
    }

    private TicketSla(
        Guid id,
        Guid ticketId,
        Guid slaPolicyId,
        DateTime responseDeadline,
        DateTime resolutionDeadline)
    {
        Id = id;
        TicketId = ticketId;
        SlaPolicyId = slaPolicyId;
        ResponseDeadline = responseDeadline;
        ResolutionDeadline = resolutionDeadline;
    }

    public Guid Id { get; private set; }

    public Guid TicketId { get; private set; }

    public Guid SlaPolicyId { get; private set; }

    public DateTime ResponseDeadline { get; private set; }

    public DateTime ResolutionDeadline { get; private set; }

    public bool ResponseBreached { get; private set; }

    public bool ResolutionBreached { get; private set; }

    public static TicketSla Create(
        Guid ticketId,
        SlaPolicy policy,
        DateTime createdAtUtc)
    {
        if (ticketId == Guid.Empty)
            throw new DomainException(
                "Ticket is required.");

        var responseDeadline =
            createdAtUtc.AddMinutes(
                policy.ResponseTimeMinutes);

        var resolutionDeadline =
            createdAtUtc.AddMinutes(
                policy.ResolutionTimeMinutes);

        return new TicketSla(
            Guid.NewGuid(),
            ticketId,
            policy.Id,
            responseDeadline,
            resolutionDeadline);
    }

    public void MarkResponseBreached()
    {
        ResponseBreached = true;
    }

    public void MarkResolutionBreached()
    {
        ResolutionBreached = true;
    }
}