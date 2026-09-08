using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;

namespace SupportCenter.Application.Features.Tickets.GetTicket;

public sealed class GetTicketQueryHandler
    : IQueryHandler<GetTicketQuery, TicketDto?>
{
    private readonly ITicketReadRepository _repository;


    public GetTicketQueryHandler(
        ITicketReadRepository repository)
    {
        _repository = repository;
    }


    public async Task<TicketDto?> Handle(
        GetTicketQuery query,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(
            query.Id,
            cancellationToken);
    }
}