using Microsoft.EntityFrameworkCore;
using SupportCenter.Application.Abstractions.Repositories;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class PermissionRepository
    : IPermissionRepository
{
    private readonly AppDbContext _context;


    public PermissionRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task<bool> UserHasPermissionAsync(
        Guid userId,
        string permission,
        CancellationToken cancellationToken)
    {
        return await _context.UserRoles
            .Where(x => x.UserId == userId)
            .Join(
                _context.RolePermissions,
                userRole => userRole.RoleId,
                rolePermission => rolePermission.RoleId,
                (userRole, rolePermission) => rolePermission)
            .Join(
                _context.Permissions,
                rolePermission => rolePermission.PermissionId,
                permissionEntity => permissionEntity.Id,
                (rolePermission, permissionEntity) => permissionEntity)
            .AnyAsync(
                x => x.Name == permission,
                cancellationToken);
    }
}