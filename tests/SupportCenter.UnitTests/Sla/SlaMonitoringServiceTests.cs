using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Sla;
using SupportCenter.Domain.Sla;

namespace SupportCenter.UnitTests.Sla;

public class SlaMonitoringServiceTests
{
    [Fact]
    public async Task Should_mark_overdue_ticket_sla_as_breached()
    {
        // Arrange
        var readRepository =
            Substitute.For<ITicketSlaReadRepository>();

        var writeRepository =
            Substitute.For<ITicketSlaRepository>();

        var ticketSla = TicketSla.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            30,
            480);

        readRepository
            .GetOverdueAsync(
                Arg.Any<DateTime>(),
                Arg.Any<CancellationToken>())
            .Returns([ticketSla]);

        var service = new SlaMonitoringService(
            readRepository,
            writeRepository);


        // Act
        await service.CheckAsync(
            CancellationToken.None);


        // Assert
        Assert.True(
            ticketSla.IsBreached);

        Assert.NotNull(
            ticketSla.BreachedDate);

        await writeRepository
            .Received(1)
            .UpdateAsync(
                ticketSla,
                Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task Should_not_update_when_no_sla_is_overdue()
    {
        // Arrange
        var readRepository =
            Substitute.For<ITicketSlaReadRepository>();

        var writeRepository =
            Substitute.For<ITicketSlaRepository>();

        readRepository
            .GetOverdueAsync(
                Arg.Any<DateTime>(),
                Arg.Any<CancellationToken>())
            .Returns([]);

        var service = new SlaMonitoringService(
            readRepository,
            writeRepository);


        // Act
        await service.CheckAsync(
            CancellationToken.None);


        // Assert
        await writeRepository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<TicketSla>(),
                Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task Should_update_each_overdue_sla()
    {
        // Arrange
        var readRepository =
            Substitute.For<ITicketSlaReadRepository>();

        var writeRepository =
            Substitute.For<ITicketSlaRepository>();

        var firstTicketSla = TicketSla.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            30,
            480);

        var secondTicketSla = TicketSla.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            60,
            240);

        readRepository
            .GetOverdueAsync(
                Arg.Any<DateTime>(),
                Arg.Any<CancellationToken>())
            .Returns(
                [
                    firstTicketSla,
                    secondTicketSla
                ]);

        var service = new SlaMonitoringService(
            readRepository,
            writeRepository);


        // Act
        await service.CheckAsync(
            CancellationToken.None);


        // Assert
        Assert.True(
            firstTicketSla.IsBreached);

        Assert.True(
            secondTicketSla.IsBreached);

        await writeRepository
            .Received(1)
            .UpdateAsync(
                firstTicketSla,
                Arg.Any<CancellationToken>());

        await writeRepository
            .Received(1)
            .UpdateAsync(
                secondTicketSla,
                Arg.Any<CancellationToken>());
    }
}