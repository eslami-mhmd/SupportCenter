using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SupportCenter.Application.Features.Notifications.Processing;

namespace SupportCenter.Infrastructure.BackgroundJobs;

public sealed class NotificationProcessingWorker
    : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationProcessingWorker> _logger;


    public NotificationProcessingWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<NotificationProcessingWorker> logger)
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


                var processor =
                    scope.ServiceProvider
                        .GetRequiredService<NotificationProcessingService>();


                await processor.ProcessAsync(
                    stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Notification processing failed.");
            }


            await Task.Delay(
                TimeSpan.FromSeconds(30),
                stoppingToken);
        }
    }
}