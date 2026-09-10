using SupportCenter.Domain.Notifications;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface INotificationRepository
{
    Task AddAsync(
        Notification notification,
        CancellationToken cancellationToken);
}