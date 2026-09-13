using Microsoft.AspNetCore.Identity;
using SupportCenter.Domain.Permissions;

namespace SupportCenter.Infrastructure.Identity;

public sealed class RolePermission
{
    private RolePermission()
    {
    }


    public RolePermission(
        Guid roleId,
        Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }


    public Guid RoleId { get; private set; }


    public Guid PermissionId { get; private set; }


    public IdentityRole<Guid> Role { get; private set; } = null!;


    public Permission Permission { get; private set; } = null!;
}