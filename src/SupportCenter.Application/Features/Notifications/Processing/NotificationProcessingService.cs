using SupportCenter.Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;

namespace SupportCenter.Application.Features.Notifications.Processing;

public sealed class NotificationProcessingService
{
    private readonly INotificationReadRepository _readRepository;
    private readonly INotificationRepository _repository;
    private readonly ILogger<NotificationProcessingService> _logger;


    public NotificationProcessingService(
        INotificationReadRepository readRepository,
        INotificationRepository repository,
        ILogger<NotificationProcessingService> logger)
    {
        _readRepository = readRepository;
        _repository = repository;
        _logger = logger;
    }


    public async Task ProcessAsync(
        CancellationToken cancellationToken)
    {
        var notifications =
            await _readRepository.GetPendingAsync(
                cancellationToken);


        foreach (var notification in notifications)
        {
            // Future:
            // Email provider
            // SMS provider
            // Push provider


            notification.MarkAsProcessed();


            await _repository.UpdateAsync(
                notification,
                cancellationToken);

            _logger.LogInformation(
                "Processed notification {NotificationId}.",
                notification.Id);
        }
    }
}