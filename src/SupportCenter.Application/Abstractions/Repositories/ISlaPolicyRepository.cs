using SupportCenter.Domain.Sla;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface ISlaPolicyRepository
{
    Task AddAsync(
        SlaPolicy policy,
        CancellationToken cancellationToken);

    Task<SlaPolicy?> GetAsync(
        Guid organizationId,
        TicketPriority priority,
        CancellationToken cancellationToken);
}