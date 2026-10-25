using SupportCenter.Application.Abstractions.Identity;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Exceptions;
using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Tickets.GetTicket;

public sealed class GetTicketQueryHandler
    : IQueryHandler<GetTicketQuery, TicketDto>
{
    private readonly ITicketReadRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public GetTicketQueryHandler(
        ITicketReadRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<TicketDto> Handle(
        GetTicketQuery query,
        CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByIdAsync(
            query.Id,
            _currentUser.OrganizationId,
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