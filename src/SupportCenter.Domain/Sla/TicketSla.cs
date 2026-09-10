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
        IsBreached = false;
    }


    public Guid Id { get; private set; }


    public Guid TicketId { get; private set; }


    public Guid SlaPolicyId { get; private set; }


    public DateTime ResponseDeadline { get; private set; }


    public DateTime ResolutionDeadline { get; private set; }

    public bool IsBreached { get; private set; }

    public DateTime? BreachedDate { get; private set; }


    public static TicketSla Create(
        Guid ticketId,
        Guid slaPolicyId,
        int responseTimeMinutes,
        int resolutionTimeMinutes)
    {
        if (ticketId == Guid.Empty)
            throw new DomainException(
                "Ticket is required.");


        if (slaPolicyId == Guid.Empty)
            throw new DomainException(
                "SLA policy is required.");


        if (responseTimeMinutes <= 0)
            throw new DomainException(
                "Response time must be positive.");


        if (resolutionTimeMinutes <= 0)
            throw new DomainException(
                "Resolution time must be positive.");


        var now = DateTime.UtcNow;


        return new TicketSla(
            Guid.NewGuid(),
            ticketId,
            slaPolicyId,
            now.AddMinutes(responseTimeMinutes),
            now.AddMinutes(resolutionTimeMinutes));
    }



    public void MarkAsBreached()
    {
        if (IsBreached)
            return;

        IsBreached = true;
        BreachedDate = DateTime.UtcNow;
    }
}