using SupportCenter.Application.Features.Tickets.GetTicket;
using SupportCenter.Application.Common.Pagination;
using SupportCenter.Application.Features.Tickets.ListTickets;
namespace SupportCenter.Application.Abstractions.Repositories;

public interface ITicketReadRepository
{
    Task<TicketDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
    Task<PagedResult<TicketDto>> ListAsync(
        ListTicketsQuery query,
        CancellationToken cancellationToken);
}