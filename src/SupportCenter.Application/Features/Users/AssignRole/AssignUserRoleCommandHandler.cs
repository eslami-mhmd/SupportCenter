using SupportCenter.Application.Abstractions.Identity;

namespace SupportCenter.Application.Features.Users.AssignRole;

public sealed class AssignUserRoleCommandHandler
{
    private readonly IUserRoleService _userRoleService;


    public AssignUserRoleCommandHandler(
        IUserRoleService userRoleService)
    {
        _userRoleService = userRoleService;
    }


    public async Task Handle(
        AssignUserRoleCommand command,
        CancellationToken cancellationToken)
    {
        await _userRoleService.AssignRoleAsync(
            command.UserId,
            command.RoleName,
            cancellationToken);
    }
}