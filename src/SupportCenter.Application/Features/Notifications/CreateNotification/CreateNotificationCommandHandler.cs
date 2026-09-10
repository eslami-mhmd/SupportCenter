using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Notifications;

namespace SupportCenter.Application.Features.Notifications.CreateNotification;

public sealed class CreateNotificationCommandHandler
    : ICommandHandler<CreateNotificationCommand, Guid>
{
    private readonly INotificationRepository _repository;


    public CreateNotificationCommandHandler(
        INotificationRepository repository)
    {
        _repository = repository;
    }


    public async Task<Guid> Handle(
        CreateNotificationCommand command,
        CancellationToken cancellationToken)
    {
        var notification =
            Notification.Create(
                command.OrganizationId,
                command.UserId,
                command.Type,
                command.Message);


        await _repository.AddAsync(
            notification,
            cancellationToken);


        return notification.Id;
    }
}