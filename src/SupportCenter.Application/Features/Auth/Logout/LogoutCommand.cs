using SupportCenter.Application.Abstractions.Identity;
using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Auth.Logout;

public sealed record LogoutCommand : ICommand<bool>;

public sealed class LogoutCommandHandler(
    IUserAuthenticationService authenticationService)
    : ICommandHandler<LogoutCommand, bool>
{
    public async Task<bool> Handle(
        LogoutCommand command,
        CancellationToken cancellationToken)
    {
        await authenticationService.SignOutAsync(cancellationToken);

        return true;
    }
}