using SupportCenter.Application.Abstractions.Identity;
using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Auth.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : ICommand<Guid>;

public sealed class LoginCommandHandler(
    IUserAuthenticationService authenticationService)
    : ICommandHandler<LoginCommand, Guid>
{
    public Task<Guid> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        return authenticationService.LoginAsync(
            command.Email,
            command.Password,
            cancellationToken);
    }
}