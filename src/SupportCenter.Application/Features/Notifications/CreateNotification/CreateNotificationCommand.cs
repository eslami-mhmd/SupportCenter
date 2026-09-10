using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Notifications.CreateNotification;

public sealed record CreateNotificationCommand(
    Guid OrganizationId,
    Guid? UserId,
    string Type,
    string Message)
    : ICommand<Guid>;