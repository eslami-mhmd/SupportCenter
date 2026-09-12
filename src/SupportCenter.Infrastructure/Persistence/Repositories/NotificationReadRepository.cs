using Microsoft.EntityFrameworkCore;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Notifications;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class NotificationReadRepository
    : INotificationReadRepository
{
    private readonly AppDbContext _context;


    public NotificationReadRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task<IReadOnlyList<Notification>> GetPendingAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .Where(x => !x.IsProcessed)
            .OrderBy(x => x.CreatedDate)
            .ToListAsync(cancellationToken);
    }
}