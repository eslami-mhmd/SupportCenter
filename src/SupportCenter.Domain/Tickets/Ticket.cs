using SupportCenter.Domain.Exceptions;
namespace SupportCenter.Domain.Tickets;

public sealed class Ticket
{
    private Ticket()
    {
    }

    private Ticket(
        Guid id,
        Guid organizationId,
        string title,
        string description)
    {
        Id = id;
        OrganizationId = organizationId;
        Title = title;
        Description = description;
        Status = TicketStatus.Open;
        Priority = TicketPriority.Medium;
        CreatedDate = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public TicketStatus Status { get; private set; }

    public TicketPriority Priority { get; private set; }

    public DateTime CreatedDate { get; private set; }


    public static Ticket Create(
        Guid organizationId,
        string title,
        string description)
    {
        if (organizationId == Guid.Empty)
            throw new DomainException(
                "Organization is required.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException(
                "Title is required.");

        return new Ticket(
            Guid.NewGuid(),
            organizationId,
            title,
            description);
    }
}