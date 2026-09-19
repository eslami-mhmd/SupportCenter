using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupportCenter.Domain.Permissions;
using SupportCenter.Infrastructure.Persistence;
using SupportCenter.Application.Security;

namespace SupportCenter.Infrastructure.Identity;

public sealed class RbacSeeder
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly AppDbContext _dbContext;


    public RbacSeeder(
        RoleManager<IdentityRole<Guid>> roleManager,
        AppDbContext dbContext)
    {
        _roleManager = roleManager;
        _dbContext = dbContext;
    }


    public async Task SeedAsync()
    {
        await SeedRolesAsync();

        await SeedPermissionsAsync();

        await SeedRolePermissionsAsync();
    }


    private async Task SeedRolesAsync()
    {
        var roles = new[]
        {
            Roles.Admin,
            Roles.SupportAgent,
            Roles.Customer
        };


        foreach (var roleName in roles)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole<Guid>(roleName));
            }
        }
    }


    private async Task SeedPermissionsAsync()
    {
        var permissions = new[]
        {
            Permissions.TicketsCreate,
            Permissions.TicketsView,
            Permissions.TicketsAssign,
            Permissions.TicketsChangeStatus,
            Permissions.UsersManage,
            Permissions.RolesManage
        };


        foreach (var name in permissions)
        {
            var exists =
                await _dbContext.Permissions
                    .AnyAsync(x => x.Name == name);


            if (!exists)
            {
                _dbContext.Permissions.Add(
                    Permission.Create(name));
            }
        }


        await _dbContext.SaveChangesAsync();
    }


    private async Task SeedRolePermissionsAsync()
    {
        var admin =
            await _roleManager.FindByNameAsync(
                Roles.Admin);


        if (admin is null)
            return;


        var permissions =
            await _dbContext.Permissions.ToListAsync();


        foreach (var permission in permissions)
        {
            var exists =
                await _dbContext.RolePermissions
                    .AnyAsync(x =>
                        x.RoleId == admin.Id &&
                        x.PermissionId == permission.Id);


            if (!exists)
            {
                _dbContext.RolePermissions.Add(
                    new RolePermission(
                        admin.Id,
                        permission.Id));
            }
        }


        await _dbContext.SaveChangesAsync();
    }
}