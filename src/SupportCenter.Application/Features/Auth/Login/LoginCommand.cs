using SupportCenter.Application.Abstractions.Identity;
using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Auth.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : ICommand<LoginResult>;

public sealed class LoginCommandHandler(
    IUserAuthenticationService authenticationService)
    : ICommandHandler<LoginCommand, LoginResult>
{
    public Task<LoginResult> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        return authenticationService.LoginAsync(
            command.Email,
            command.Password,
            cancellationToken);
    }
}