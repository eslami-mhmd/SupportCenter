using SupportCenter.Domain.Auditing;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface IAuditRepository
{
    Task AddAsync(
        AuditEntry auditEntry,
        CancellationToken cancellationToken);
}