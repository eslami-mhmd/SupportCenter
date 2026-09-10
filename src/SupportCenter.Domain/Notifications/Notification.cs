namespace SupportCenter.Domain.Notifications;

public sealed class Notification
{
    private Notification()
    {
    }


    private Notification(
        Guid id,
        Guid organizationId,
        Guid? userId,
        string type,
        string message)
    {
        Id = id;
        OrganizationId = organizationId;
        UserId = userId;
        Type = type;
        Message = message;
        IsRead = false;
        CreatedDate = DateTime.UtcNow;
    }


    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public Guid? UserId { get; private set; }

    public string Type { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public bool IsRead { get; private set; }

    public DateTime CreatedDate { get; private set; }


    public static Notification Create(
        Guid organizationId,
        Guid? userId,
        string type,
        string message)
    {
        return new Notification(
            Guid.NewGuid(),
            organizationId,
            userId,
            type,
            message);
    }


    public void MarkAsRead()
    {
        IsRead = true;
    }
}