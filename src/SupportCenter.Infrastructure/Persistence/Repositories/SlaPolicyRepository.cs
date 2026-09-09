using Microsoft.EntityFrameworkCore;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Sla;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class SlaPolicyRepository
    : ISlaPolicyRepository
{
    private readonly AppDbContext _context;

    public SlaPolicyRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        SlaPolicy policy,
        CancellationToken cancellationToken)
    {
        await _context.SlaPolicies.AddAsync(
            policy,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public Task<SlaPolicy?> GetAsync(
        Guid organizationId,
        TicketPriority priority,
        CancellationToken cancellationToken)
    {
        return _context.SlaPolicies
            .FirstOrDefaultAsync(
                x =>
                    x.OrganizationId == organizationId &&
                    x.Priority == priority,
                cancellationToken);
    }
}