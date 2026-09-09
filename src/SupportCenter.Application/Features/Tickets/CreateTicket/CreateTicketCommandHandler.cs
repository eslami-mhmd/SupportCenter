using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Application.Features.Tickets.CreateTicket;

public sealed class CreateTicketCommandHandler
    : ICommandHandler<CreateTicketCommand, Guid>
{
    private readonly ITicketWriteRepository _repository;

    public CreateTicketCommandHandler(
        ITicketWriteRepository repository)
    {
        _repository = repository;
    }


    public async Task<Guid> Handle(
        CreateTicketCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = Ticket.Create(
            command.OrganizationId,
            command.Title,
            command.Description);


        await _repository.AddAsync(
            ticket,
            cancellationToken);


        return ticket.Id;
    }
}