
using SupportCenter.Application.Abstractions.Identity;
using SupportCenter.Application.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace SupportCenter.Infrastructure.Identity;

public sealed class UserService(
    UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<Guid> CreateUserAsync(
        string email,
        string password,
        Guid? organizationId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            OrganizationId = organizationId
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .GroupBy(error => error.Code)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(error => error.Description)
                        .ToArray());

            throw new ValidationException(errors);
        }

        return user.Id;
    }
}
