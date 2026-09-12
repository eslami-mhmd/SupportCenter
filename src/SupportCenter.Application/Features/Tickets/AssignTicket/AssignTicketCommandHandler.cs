using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Auditing.CreateAuditEntry;

namespace SupportCenter.Application.Features.Tickets.AssignTicket;

public sealed class AssignTicketCommandHandler
    : ICommandHandler<AssignTicketCommand, bool>
{
    private readonly ITicketWriteRepository _repository;
    private readonly IDispatcher _dispatcher;
    public AssignTicketCommandHandler(
        ITicketWriteRepository repository,
        IDispatcher dispatcher)
    {
        _repository = repository;
        _dispatcher = dispatcher;
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


        var previousAssigneeId =
            ticket.AssignedUserId;

        ticket.AssignTo(
            command.UserId);


        await _repository.UpdateAsync(
            ticket,
            cancellationToken);

        await _dispatcher.Send<Guid>(
            new CreateAuditEntryCommand(
                null,
                "TICKET_ASSIGNED",
                "Ticket",
                ticket.Id,
                $"{{\"AssignedUserId\":\"{previousAssigneeId}\"}}",
                $"{{\"AssignedUserId\":\"{ticket.AssignedUserId}\"}}"),
            cancellationToken);

        return true;
    }
}