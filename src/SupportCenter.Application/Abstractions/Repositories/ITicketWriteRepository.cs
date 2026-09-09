using SupportCenter.Domain.Tickets;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface ITicketWriteRepository
{
    Task<Ticket?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task AddAsync(
        Ticket ticket,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Ticket ticket,
        CancellationToken cancellationToken);
}