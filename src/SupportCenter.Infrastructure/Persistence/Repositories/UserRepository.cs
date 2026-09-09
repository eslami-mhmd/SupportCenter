using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Users;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;


    public UserRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(
            user,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}