using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Tickets;
using SupportCenter.Domain.Sla;
using SupportCenter.Application.Features.Auditing.CreateAuditEntry;

namespace SupportCenter.Application.Features.Tickets.CreateTicket;

public sealed class CreateTicketCommandHandler
    : ICommandHandler<CreateTicketCommand, Guid>
{
    private readonly ITicketWriteRepository _ticketRepository;
    private readonly ITicketSlaRepository _ticketSlaRepository;
    private readonly ISlaPolicyRepository _slaPolicyRepository;
    private readonly IDispatcher _dispatcher;
    public CreateTicketCommandHandler(
        ITicketWriteRepository ticketRepository,
        ISlaPolicyRepository slaPolicyRepository,
        ITicketSlaRepository ticketSlaRepository,
        IDispatcher dispatcher)
    {
        _ticketRepository = ticketRepository;
        _slaPolicyRepository = slaPolicyRepository;
        _ticketSlaRepository = ticketSlaRepository;
        _dispatcher = dispatcher;
    }

    public async Task<Guid> Handle(
        CreateTicketCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = Ticket.Create(
            command.OrganizationId,
            command.Title,
            command.Description);


        await _ticketRepository.AddAsync(
            ticket,
            cancellationToken);

        await _dispatcher.Send<Guid>(
            new CreateAuditEntryCommand(
                null,
                "TICKET_CREATED",
                "Ticket",
                ticket.Id,
                null,
                $"{{\"Title\":\"{ticket.Title}\"}}"),
            cancellationToken);


        var slaPolicy =
            await _slaPolicyRepository.GetAsync(
                command.OrganizationId,
                ticket.Priority,
                cancellationToken);



        if (slaPolicy is not null)
        {
            var ticketSla =
                TicketSla.Create(
                    ticket.Id,
                    slaPolicy.Id,
                    slaPolicy.ResponseTimeMinutes,
                    slaPolicy.ResolutionTimeMinutes);


            await _ticketSlaRepository.AddAsync(
                ticketSla,
                cancellationToken);
        }


        return ticket.Id;
    }
}