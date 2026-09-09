using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Exceptions;

namespace SupportCenter.Application.Features.Tickets.ChangeTicketStatus;

public sealed class ChangeTicketStatusCommandHandler
    : ICommandHandler<ChangeTicketStatusCommand, Guid>
{
    private readonly ITicketWriteRepository _repository;


    public ChangeTicketStatusCommandHandler(
        ITicketWriteRepository repository)
    {
        _repository = repository;
    }


    public async Task<Guid> Handle(
        ChangeTicketStatusCommand command,
        CancellationToken cancellationToken)
    {
        var ticket =
            await _repository.GetAsync(
                command.TicketId,
                cancellationToken);


        if (ticket is null)
        {
            throw new NotFoundException(
                "Ticket", command.TicketId);
        }


        ticket.ChangeStatus(
            command.Status);


        await _repository.UpdateAsync(
            ticket,
            cancellationToken);

        return ticket.Id;
    }
}