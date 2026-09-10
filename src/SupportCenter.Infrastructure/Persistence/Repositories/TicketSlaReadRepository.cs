using Microsoft.EntityFrameworkCore;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Sla;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class TicketSlaReadRepository
    : ITicketSlaReadRepository
{
    private readonly AppDbContext _context;


    public TicketSlaReadRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task<IReadOnlyList<TicketSla>> GetOverdueAsync(
        DateTime utcNow,
        CancellationToken cancellationToken)
    {
        return await _context.TicketSlas
            .Where(x =>
                !x.IsBreached &&
                (
                    x.ResponseDeadline < utcNow ||
                    x.ResolutionDeadline < utcNow
                ))
            .ToListAsync(cancellationToken);
    }
}