using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Exceptions;
using SupportCenter.Application.Features.Auditing.CreateAuditEntry;

namespace SupportCenter.Application.Features.Tickets.ChangeTicketStatus;

public sealed class ChangeTicketStatusCommandHandler
    : ICommandHandler<ChangeTicketStatusCommand, Guid>
{
    private readonly ITicketWriteRepository _repository;
    private readonly IDispatcher _dispatcher;

    public ChangeTicketStatusCommandHandler(
        ITicketWriteRepository repository,
        IDispatcher dispatcher)
    {
        _repository = repository;
        _dispatcher = dispatcher;
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


        var oldStatus =
            ticket.Status;


        ticket.ChangeStatus(
            command.Status);


        await _repository.UpdateAsync(
            ticket,
            cancellationToken);


        await _dispatcher.Send<Guid>(
            new CreateAuditEntryCommand(
                null,
                "STATUS_CHANGED",
                "Ticket",
                ticket.Id,
                $"{{\"Status\":\"{oldStatus}\"}}",
                $"{{\"Status\":\"{ticket.Status}\"}}"),
            cancellationToken);

        return ticket.Id;
    }
}