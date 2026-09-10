using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Abstractions.Sla;
using SupportCenter.Application.Features.Notifications.CreateNotification;
using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Sla;

public sealed class SlaMonitoringService
    : ISlaMonitoringService
{
    private readonly ITicketSlaReadRepository _ticketSlaReadRepository;
    private readonly ITicketSlaRepository _ticketSlaRepository;
    private readonly ITicketReadRepository _ticketReadRepository;
    private readonly IDispatcher _dispatcher;
    public SlaMonitoringService(
        ITicketSlaReadRepository ticketSlaReadRepository,
        ITicketSlaRepository ticketSlaRepository,
        ITicketReadRepository ticketReadRepository,
        IDispatcher dispatcher)
    {
        _ticketSlaReadRepository = ticketSlaReadRepository;
        _ticketSlaRepository = ticketSlaRepository;
        _ticketReadRepository = ticketReadRepository;
        _dispatcher = dispatcher;
    }
    public async Task CheckAsync(
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;


        var overdueSlas =
            await _ticketSlaReadRepository.GetOverdueAsync(
                now,
                cancellationToken);


        foreach (var ticketSla in overdueSlas)
        {
            ticketSla.MarkAsBreached();

            await _ticketSlaRepository.UpdateAsync(
                ticketSla,
                cancellationToken);


            var ticket =
                await _ticketReadRepository.GetByIdAsync(
                    ticketSla.TicketId,
                    cancellationToken);


            if (ticket is null)
            {
                continue;
            }


            await _dispatcher.Send<Guid>(
                new CreateNotificationCommand(
                    ticket.OrganizationId,
                    null,
                    "SLA_BREACH",
                    $"Ticket '{ticket.Title}' exceeded its SLA target."),
                cancellationToken);
        }
    }
}