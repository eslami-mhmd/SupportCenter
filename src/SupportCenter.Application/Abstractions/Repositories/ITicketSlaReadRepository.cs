using SupportCenter.Domain.Sla;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface ITicketSlaReadRepository
{
    Task<IReadOnlyList<TicketSla>> GetOverdueAsync(
        DateTime utcNow,
        CancellationToken cancellationToken);
}