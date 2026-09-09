using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;

namespace SupportCenter.Application.Features.Tickets.AssignTicket;

public sealed class AssignTicketCommandHandler
    : ICommandHandler<AssignTicketCommand, bool>
{
    private readonly ITicketWriteRepository _repository;


    public AssignTicketCommandHandler(
        ITicketWriteRepository repository)
    {
        _repository = repository;
    }


    public async Task<bool> Handle(
        AssignTicketCommand command,
        CancellationToken cancellationToken)
    {
        var ticket =
            await _repository.GetAsync(
                command.TicketId,
                cancellationToken);


        if (ticket is null)
        {
            return false;
        }


        ticket.AssignTo(
            command.UserId);


        await _repository.UpdateAsync(
            ticket,
            cancellationToken);


        return true;
    }
}