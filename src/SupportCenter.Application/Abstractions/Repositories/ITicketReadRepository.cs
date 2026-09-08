using SupportCenter.Application.Features.Tickets.GetTicket;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface ITicketReadRepository
{
    Task<TicketDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
}