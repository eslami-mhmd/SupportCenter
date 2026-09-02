using System.Collections.Concurrent;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Organizations;

namespace SupportCenter.Infrastructure.Persistence;

public sealed class InMemoryOrganizationRepository
    : IOrganizationRepository
{
    private readonly ConcurrentDictionary<Guid, Organization> _organizations = [];

    public Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken)
    {
        _organizations[organization.Id] = organization;

        return Task.CompletedTask;
    }
}