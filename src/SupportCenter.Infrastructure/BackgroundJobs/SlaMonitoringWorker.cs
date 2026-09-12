using SupportCenter.Application.Abstractions.Sla;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SupportCenter.Infrastructure.BackgroundJobs;

public sealed class SlaMonitoringWorker
    : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SlaMonitoringWorker> _logger;


    public SlaMonitoringWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<SlaMonitoringWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope =
                    _scopeFactory.CreateScope();


                var service =
                    scope.ServiceProvider
                        .GetRequiredService<ISlaMonitoringService>();


                await service.CheckAsync(
                    stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while checking SLA violations.");
            }


            await Task.Delay(
                TimeSpan.FromMinutes(1),
                stoppingToken);
        }
    }
}