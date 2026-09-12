using SupportCenter.Domain.Notifications;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface INotificationReadRepository
{
    Task<IReadOnlyList<Notification>> GetPendingAsync(
        CancellationToken cancellationToken);
}