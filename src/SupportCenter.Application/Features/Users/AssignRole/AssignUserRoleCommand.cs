namespace SupportCenter.Application.Features.Users.AssignRole;

public sealed record AssignUserRoleCommand(
    Guid UserId,
    string RoleName);