using SupportCenter.Application.Abstractions.Repositories;

namespace SupportCenter.Application.Features.Notifications.Processing;

public sealed class NotificationProcessingService
{
    private readonly INotificationReadRepository _readRepository;
    private readonly INotificationRepository _repository;


    public NotificationProcessingService(
        INotificationReadRepository readRepository,
        INotificationRepository repository)
    {
        _readRepository = readRepository;
        _repository = repository;
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
        }
    }
}