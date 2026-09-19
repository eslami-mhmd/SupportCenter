using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupportCenter.Application.Security;
using SupportCenter.Infrastructure.Persistence;

namespace SupportCenter.Infrastructure.Identity;

public sealed class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly AppDbContext _dbContext;


    public PermissionAuthorizationHandler(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userIdClaim =
            context.User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);


        if (userIdClaim is null)
            return;


        if (!Guid.TryParse(
                userIdClaim.Value,
                out var userId))
        {
            return;
        }


        var hasPermission =
            await _dbContext.UserRoles
                .Where(x => x.UserId == userId)
                .Join(
                    _dbContext.RolePermissions,
                    userRole => userRole.RoleId,
                    rolePermission => rolePermission.RoleId,
                    (userRole, rolePermission) =>
                        rolePermission)
                .Join(
                    _dbContext.Permissions,
                    rolePermission => rolePermission.PermissionId,
                    permission => permission.Id,
                    (rolePermission, permission) =>
                        permission.Name)
                .AnyAsync(
                    x => x == requirement.Permission);


        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}