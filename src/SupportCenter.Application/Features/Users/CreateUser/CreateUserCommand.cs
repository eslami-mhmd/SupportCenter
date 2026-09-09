using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Domain.Users;

namespace SupportCenter.Application.Features.Users.CreateUser;

public sealed record CreateUserCommand(
    Guid OrganizationId,
    string Email,
    string DisplayName,
    UserRole Role)
    : ICommand<Guid>;