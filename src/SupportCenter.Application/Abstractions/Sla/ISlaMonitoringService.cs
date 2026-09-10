namespace SupportCenter.Application.Abstractions.Sla;

public interface ISlaMonitoringService
{
    Task CheckAsync(
        CancellationToken cancellationToken);
}