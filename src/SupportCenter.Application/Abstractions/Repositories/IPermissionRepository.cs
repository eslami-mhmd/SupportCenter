namespace SupportCenter.Application.Abstractions.Repositories;

public interface IPermissionRepository
{
    Task<bool> UserHasPermissionAsync(
        Guid userId,
        string permission,
        CancellationToken cancellationToken);
}