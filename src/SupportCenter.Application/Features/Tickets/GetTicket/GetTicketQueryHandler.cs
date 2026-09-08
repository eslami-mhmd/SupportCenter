using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Exceptions;
using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Tickets.GetTicket;

public sealed class GetTicketQueryHandler 
    : IQueryHandler<GetTicketQuery, TicketDto>
{
    private readonly ITicketReadRepository _repository;

    public GetTicketQueryHandler(
        ITicketReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketDto> Handle(
        GetTicketQuery query,
        CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByIdAsync(
            query.Id,
            cancellationToken);

        if (ticket is null)
        {
            throw new NotFoundException(
                "Ticket",
                query.Id);
        }

        return ticket;
    }
}