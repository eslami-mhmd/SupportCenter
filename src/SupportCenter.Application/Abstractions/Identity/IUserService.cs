namespace SupportCenter.Application.Abstractions.Identity;

public interface IUserService
{
    Task<Guid> CreateUserAsync(
        string email,
        string password,
        Guid? organizationId,
        CancellationToken cancellationToken);
}