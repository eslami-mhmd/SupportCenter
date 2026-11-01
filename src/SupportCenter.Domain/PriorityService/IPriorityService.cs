using SupportCenter.Domain.Tickets;

namespace SupportCenter.Domain.PriorityService;

public interface IPriorityService
{
    Task<TicketPriority> DeterminePriorityAsync(string title, string description, CancellationToken cancellationToken = default);
}
