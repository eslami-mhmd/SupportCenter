namespace SupportCenter.Application.Abstractions.Identity;

public interface IUserRoleService
{
    Task AssignRoleAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken);

    Task RemoveRoleAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken);
}