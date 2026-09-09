using SupportCenter.Domain.Exceptions;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Domain.Sla;

public sealed class SlaPolicy
{
    private SlaPolicy()
    {
    }

    private SlaPolicy(
        Guid id,
        Guid organizationId,
        TicketPriority priority,
        int responseTimeMinutes,
        int resolutionTimeMinutes)
    {
        Id = id;
        OrganizationId = organizationId;
        Priority = priority;
        ResponseTimeMinutes = responseTimeMinutes;
        ResolutionTimeMinutes = resolutionTimeMinutes;
    }

    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public TicketPriority Priority { get; private set; }

    public int ResponseTimeMinutes { get; private set; }

    public int ResolutionTimeMinutes { get; private set; }

    public static SlaPolicy Create(
        Guid organizationId,
        TicketPriority priority,
        int responseTimeMinutes,
        int resolutionTimeMinutes)
    {
        if (organizationId == Guid.Empty)
            throw new DomainException(
                "Organization is required.");

        if (responseTimeMinutes <= 0)
            throw new DomainException(
                "Response time must be positive.");

        if (resolutionTimeMinutes <= 0)
            throw new DomainException(
                "Resolution time must be positive.");

        return new SlaPolicy(
            Guid.NewGuid(),
            organizationId,
            priority,
            responseTimeMinutes,
            resolutionTimeMinutes);
    }
}