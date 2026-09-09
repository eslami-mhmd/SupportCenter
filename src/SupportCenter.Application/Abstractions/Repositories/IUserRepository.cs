using SupportCenter.Domain.Users;

namespace SupportCenter.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task AddAsync(
        User user,
        CancellationToken cancellationToken);
}