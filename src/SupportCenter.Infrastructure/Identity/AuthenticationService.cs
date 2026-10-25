using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SupportCenter.Application.Abstractions.Identity;
using SupportCenter.Application.Exceptions;

namespace SupportCenter.Infrastructure.Identity;

public sealed class AuthenticationService(
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor) : IUserAuthenticationService
{
    public async Task<Guid> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByEmailAsync(email);

        if (user is null ||
            !await userManager.CheckPasswordAsync(user, password))
        {
            throw new UnauthorizedException();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        if (user.OrganizationId is { } organizationId)
        {
            claims.Add(
                new Claim("OrganizationId", organizationId.ToString()));
        }

        claims.AddRange(
            (await userManager.GetRolesAsync(user))
                .Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await httpContextAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return user.Id;
    }

    public async Task SignOutAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await httpContextAccessor.HttpContext!.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
}