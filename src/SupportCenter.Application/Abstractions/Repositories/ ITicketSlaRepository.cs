using SupportCenter.Domain.Sla;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface ITicketSlaRepository
{
    Task AddAsync(
        TicketSla ticketSla,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TicketSla ticketSla,
        CancellationToken cancellationToken);
}