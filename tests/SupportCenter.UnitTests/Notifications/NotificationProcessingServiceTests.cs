using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Notifications.Processing;
using SupportCenter.Domain.Notifications;

namespace SupportCenter.UnitTests.Notifications;

public class NotificationProcessingServiceTests
{
    [Fact]
    public async Task Should_process_pending_notifications()
    {
        // Arrange

        var readRepository =
            Substitute.For<INotificationReadRepository>();

        var repository =
            Substitute.For<INotificationRepository>();


        var notification =
            Notification.Create(
                Guid.NewGuid(),
                null,
                "SLA_BREACH",
                "Ticket exceeded SLA target.");


        readRepository
            .GetPendingAsync(
                Arg.Any<CancellationToken>())
            .Returns(
                new List<Notification>
                {
                    notification
                });


        var service =
            new NotificationProcessingService(
                readRepository,
                repository);



        // Act

        await service.ProcessAsync(
            CancellationToken.None);



        // Assert

        Assert.True(
            notification.IsProcessed);


        Assert.NotNull(
            notification.ProcessedDate);


        await repository
            .Received(1)
            .UpdateAsync(
                notification,
                Arg.Any<CancellationToken>());
    }
}