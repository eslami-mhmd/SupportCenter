using SupportCenter.Domain.Events;

namespace SupportCenter.Domain.Common;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(Guid id)
    {
        Id = id;
    }


    public Guid Id { get; protected set; }


    public IReadOnlyCollection<IDomainEvent> DomainEvents =>
        _domainEvents.AsReadOnly();


    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }


    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }


    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        return Id == other.Id;
    }


    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}