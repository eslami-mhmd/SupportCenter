namespace SupportCenter.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}