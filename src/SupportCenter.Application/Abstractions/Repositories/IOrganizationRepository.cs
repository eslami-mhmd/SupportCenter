using SupportCenter.Domain.Organizations;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface IOrganizationRepository
{
    Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken);
}