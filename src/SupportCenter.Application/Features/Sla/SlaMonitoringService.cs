using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Abstractions.Sla;

namespace SupportCenter.Application.Features.Sla;

public sealed class SlaMonitoringService
    : ISlaMonitoringService
{
    private readonly ITicketSlaReadRepository _readRepository;
    private readonly ITicketSlaRepository _writeRepository;


    public SlaMonitoringService(
        ITicketSlaReadRepository readRepository,
        ITicketSlaRepository writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }


    public async Task CheckAsync(
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;


        var overdueSlas =
            await _readRepository.GetOverdueAsync(
                now,
                cancellationToken);


        foreach (var ticketSla in overdueSlas)
        {
            ticketSla.MarkAsBreached();


            await _writeRepository.UpdateAsync(
                ticketSla,
                cancellationToken);
        }
    }
}