using SupportCenter.Domain.Tickets;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface ITicketRepository
{
    Task AddAsync(
        Ticket ticket,
        CancellationToken cancellationToken);
}