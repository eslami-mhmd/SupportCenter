using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Notifications.CreateNotification;
using SupportCenter.Domain.Notifications;

namespace SupportCenter.UnitTests.Notifications;

public class CreateNotificationHandlerTests
{
    [Fact]
    public async Task Should_create_notification()
    {
        // Arrange
        var repository =
            Substitute.For<INotificationRepository>();


        Notification? createdNotification = null;


        repository
            .AddAsync(
                Arg.Do<Notification>(
                    x => createdNotification = x),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);


        var handler =
            new CreateNotificationCommandHandler(
                repository);


        var organizationId =
            Guid.NewGuid();


        var command =
            new CreateNotificationCommand(
                organizationId,
                null,
                "SLA_BREACH",
                "Ticket exceeded SLA response time.");


        // Act
        var result =
            await handler.Handle(
                command,
                CancellationToken.None);


        // Assert
        Assert.NotEqual(
            Guid.Empty,
            result);


        Assert.NotNull(
            createdNotification);


        Assert.Equal(
            organizationId,
            createdNotification.OrganizationId);


        Assert.Equal(
            "SLA_BREACH",
            createdNotification.Type);


        await repository
            .Received(1)
            .AddAsync(
                Arg.Any<Notification>(),
                Arg.Any<CancellationToken>());
    }
}