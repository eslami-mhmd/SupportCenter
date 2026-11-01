using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Exceptions;
using SupportCenter.Domain.PriorityService;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Application.Features.Tickets.UpdateTicket;

public sealed class UpdateTicketCommandHandler
    : ICommandHandler<UpdateTicketCommand, Guid>
{
    private readonly ITicketWriteRepository _repository;
    private readonly IPriorityService _priorityAgent;
    private readonly IDispatcher _dispatcher;

    public UpdateTicketCommandHandler(
        ITicketWriteRepository repository,
        IPriorityService priorityAgent,
        IDispatcher dispatcher)
    {
        _repository = repository;
        _priorityAgent = priorityAgent;
        _dispatcher = dispatcher;
    }

    public async Task<Guid> Handle(
        UpdateTicketCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetAsync(command.TicketId, cancellationToken);
        if (ticket is null)
            throw new NotFoundException("Ticket", command.TicketId);

        var titleChanged = false;
        var descriptionChanged = false;

        if (command.Title is not null && command.Title != ticket.Title)
        {
            ticket.UpdateTitle(command.Title);
            titleChanged = true;
        }

        if (command.Description is not null && command.Description != ticket.Description)
        {
            ticket.UpdateDescription(command.Description);
            descriptionChanged = true;
        }

        if (command.Priority is not null)
        {
            // Human override takes precedence
            ticket.SetPriority(command.Priority.Value);
        }
        else if (titleChanged || descriptionChanged)
        {
            // Re-determine priority via agent when content changes
            var priority = await _priorityAgent.DeterminePriorityAsync(
                ticket.Title,
                ticket.Description,
                cancellationToken);
            ticket.SetPriority(priority);
        }

        await _repository.UpdateAsync(ticket, cancellationToken);
        return ticket.Id;
    }
}
