namespace SupportCenter.Application.Abstractions.Authorization;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(
        string permission,
        CancellationToken cancellationToken);
}