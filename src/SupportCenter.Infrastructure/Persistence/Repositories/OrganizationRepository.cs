using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Organizations;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class OrganizationRepository
    : IOrganizationRepository
{
    private readonly AppDbContext _context;

    public OrganizationRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken)
    {
        await _context.Organizations.AddAsync(
            organization,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}