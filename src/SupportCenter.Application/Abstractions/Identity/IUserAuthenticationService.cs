namespace SupportCenter.Application.Abstractions.Identity;

public interface IUserAuthenticationService
{
    Task<Guid> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken);

    Task SignOutAsync(
        CancellationToken cancellationToken);
}