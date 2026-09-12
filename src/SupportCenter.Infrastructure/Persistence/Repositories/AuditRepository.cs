using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Auditing;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class AuditRepository
    : IAuditRepository
{
    private readonly AppDbContext _context;


    public AuditRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(
        AuditEntry auditEntry,
        CancellationToken cancellationToken)
    {
        await _context.AuditEntries.AddAsync(
            auditEntry,
            cancellationToken);


        await _context.SaveChangesAsync(
            cancellationToken);
    }
}