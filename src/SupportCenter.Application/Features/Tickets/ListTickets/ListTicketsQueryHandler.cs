using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Common.Pagination;
using SupportCenter.Application.Features.Tickets.GetTicket;

namespace SupportCenter.Application.Features.Tickets.ListTickets;

public sealed class ListTicketsQueryHandler
    : IQueryHandler<ListTicketsQuery, PagedResult<TicketDto>>
{
    private readonly ITicketReadRepository _repository;


    public ListTicketsQueryHandler(
        ITicketReadRepository repository)
    {
        _repository = repository;
    }


    public async Task<PagedResult<TicketDto>> Handle(
        ListTicketsQuery query,
        CancellationToken cancellationToken)
    {
        return await _repository.ListAsync(
            query,
            cancellationToken);
    }
}