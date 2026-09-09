using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Domain.Users;

namespace SupportCenter.Application.Features.Users.CreateUser;

public sealed class CreateUserCommandHandler
    : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repository;


    public CreateUserCommandHandler(
        IUserRepository repository)
    {
        _repository = repository;
    }


    public async Task<Guid> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = User.Create(
            command.OrganizationId,
            command.Email,
            command.DisplayName,
            command.Role);


        await _repository.AddAsync(
            user,
            cancellationToken);


        return user.Id;
    }
}