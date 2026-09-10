using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Tickets;
using SupportCenter.Domain.Sla;

namespace SupportCenter.Application.Features.Tickets.CreateTicket;

public sealed class CreateTicketCommandHandler
    : ICommandHandler<CreateTicketCommand, Guid>
{
    private readonly ITicketWriteRepository _ticketRepository;
    private readonly ITicketSlaRepository _ticketSlaRepository;
    private readonly ISlaPolicyRepository _slaPolicyRepository;
    public CreateTicketCommandHandler(
        ITicketWriteRepository ticketRepository,
        ISlaPolicyRepository slaPolicyRepository,
        ITicketSlaRepository ticketSlaRepository)
    {
        _ticketRepository = ticketRepository;
        _slaPolicyRepository = slaPolicyRepository;
        _ticketSlaRepository = ticketSlaRepository;
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