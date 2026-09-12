namespace SupportCenter.Domain.Auditing;

public sealed class AuditEntry
{
    private AuditEntry()
    {
    }


    private AuditEntry(
        Guid id,
        Guid? userId,
        string action,
        string entityName,
        Guid entityId,
        string? oldValues,
        string? newValues)
    {
        Id = id;
        UserId = userId;
        Action = action;
        EntityName = entityName;
        EntityId = entityId;
        OldValues = oldValues;
        NewValues = newValues;
        CreatedDate = DateTime.UtcNow;
    }


    public Guid Id { get; private set; }


    public Guid? UserId { get; private set; }


    public string Action { get; private set; } = string.Empty;


    public string EntityName { get; private set; } = string.Empty;


    public Guid EntityId { get; private set; }


    public string? OldValues { get; private set; }


    public string? NewValues { get; private set; }


    public DateTime CreatedDate { get; private set; }


    public static AuditEntry Create(
        Guid? userId,
        string action,
        string entityName,
        Guid entityId,
        string? oldValues,
        string? newValues)
    {
        return new AuditEntry(
            Guid.NewGuid(),
            userId,
            action,
            entityName,
            entityId,
            oldValues,
            newValues);
    }
}