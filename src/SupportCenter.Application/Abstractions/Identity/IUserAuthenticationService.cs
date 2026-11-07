using SupportCenter.Application.Features.Auth.Login;

namespace SupportCenter.Application.Abstractions.Identity;

public interface IUserAuthenticationService
{
    Task<LoginResult> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken);

    Task SignOutAsync(
        CancellationToken cancellationToken);
}