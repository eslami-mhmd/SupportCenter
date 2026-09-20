using Microsoft.AspNetCore.Identity;
using SupportCenter.Application.Abstractions.Identity;

namespace SupportCenter.Infrastructure.Identity;

public sealed class UserRoleService 
    : IUserRoleService
{
    private readonly UserManager<ApplicationUser> _userManager;


    public UserRoleService(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    public async Task AssignRoleAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken)
    {
        var user =
            await _userManager.FindByIdAsync(
                userId.ToString());


        if (user is null)
            throw new InvalidOperationException(
                "User not found.");


        if (!await _userManager.IsInRoleAsync(
                user,
                roleName))
        {
            await _userManager.AddToRoleAsync(
                user,
                roleName);
        }
    }


    public async Task RemoveRoleAsync(
        Guid userId,
        string roleName,
        CancellationToken cancellationToken)
    {
        var user =
            await _userManager.FindByIdAsync(
                userId.ToString());


        if (user is null)
            throw new InvalidOperationException(
                "User not found.");


        if (await _userManager.IsInRoleAsync(
                user,
                roleName))
        {
            await _userManager.RemoveFromRoleAsync(
                user,
                roleName);
        }
    }
}