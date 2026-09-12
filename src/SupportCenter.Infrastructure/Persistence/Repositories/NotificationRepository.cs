using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Notifications;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class NotificationRepository
    : INotificationRepository
{
    private readonly AppDbContext _context;


    public NotificationRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(
        Notification notification,
        CancellationToken cancellationToken)
    {
        await _context.Notifications.AddAsync(
            notification,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }


    public async Task UpdateAsync(
        Notification notification,
        CancellationToken cancellationToken)
    {
        _context.Notifications.Update(
            notification);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}