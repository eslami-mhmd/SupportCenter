using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Abstractions.Sla;
using SupportCenter.Application.Features.Notifications.CreateNotification;
using SupportCenter.Application.Abstractions.Messaging;
using Microsoft.Extensions.Logging;

namespace SupportCenter.Application.Features.Sla;

public sealed class SlaMonitoringService
    : ISlaMonitoringService
{
    private readonly ITicketSlaReadRepository _ticketSlaReadRepository;
    private readonly ITicketSlaRepository _ticketSlaRepository;
    private readonly ITicketReadRepository _ticketReadRepository;
    private readonly IDispatcher _dispatcher;
    private readonly ILogger<SlaMonitoringService> _logger;
    public SlaMonitoringService(
        ITicketSlaReadRepository ticketSlaReadRepository,
        ITicketSlaRepository ticketSlaRepository,
        ITicketReadRepository ticketReadRepository,
        IDispatcher dispatcher,
        ILogger<SlaMonitoringService> logger)
    {
        _ticketSlaReadRepository = ticketSlaReadRepository;
        _ticketSlaRepository = ticketSlaRepository;
        _ticketReadRepository = ticketReadRepository;
        _dispatcher = dispatcher;
        _logger = logger;
    }
    public async Task CheckAsync(
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var overdueSlas =
            await _ticketSlaReadRepository.GetOverdueAsync(
                now,
                cancellationToken);

        _logger.LogInformation(
            "SLA check found {Count} overdue SLA(s).",
            overdueSlas.Count);

        foreach (var ticketSla in overdueSlas)
        {
            ticketSla.MarkAsBreached();

            await _ticketSlaRepository.UpdateAsync(
                ticketSla,
                cancellationToken);


            var ticket =
                await _ticketReadRepository.GetByIdAsync(
                    ticketSla.TicketId,
                    null,
                    cancellationToken);


            if (ticket is null)
            {
                continue;
            }


            _logger.LogInformation(
                "SLA exceeded for ticket {TicketId} ('{Title}').",
                ticket.Id,
                ticket.Title);

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