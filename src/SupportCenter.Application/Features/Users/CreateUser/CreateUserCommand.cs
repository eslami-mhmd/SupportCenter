using SupportCenter.Application.Abstractions.Identity;
using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Users.CreateUser;

public sealed record CreateUserCommand(
    string Email,
    string Password,
    Guid? OrganizationId) : ICommand<Guid>;

public sealed class CreateUserCommandHandler(
    IUserService userService)
    : ICommandHandler<CreateUserCommand, Guid>
{
    public Task<Guid> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        return userService.CreateUserAsync(
            command.Email,
            command.Password,
            command.OrganizationId,
            cancellationToken);
    }
}