using NSubstitute;
using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Notifications.CreateNotification;
using SupportCenter.Application.Features.Sla;
using SupportCenter.Application.Features.Tickets.GetTicket;
using SupportCenter.Domain.Sla;

namespace SupportCenter.UnitTests.Sla;

public class SlaMonitoringServiceTests
{
    [Fact]
    public async Task Should_mark_breached_sla_and_create_notification()
    {
        // Arrange

        var slaReadRepository =
            Substitute.For<ITicketSlaReadRepository>();

        var slaRepository =
            Substitute.For<ITicketSlaRepository>();

        var ticketReadRepository =
            Substitute.For<ITicketReadRepository>();

        var dispatcher =
            Substitute.For<IDispatcher>();


        var ticketId =
            Guid.NewGuid();

        var organizationId =
            Guid.NewGuid();


        var ticketSla =
            TicketSla.Create(
                ticketId,
                Guid.NewGuid(),
                10,
                60);


        slaReadRepository
            .GetOverdueAsync(
                Arg.Any<DateTime>(),
                Arg.Any<CancellationToken>())
            .Returns(
                new List<TicketSla>
                {
                    ticketSla
                });


        var ticket =
            new TicketDto(
                ticketId,
                organizationId,
                "Cannot login",
                "User cannot access system",
                "Open",
                "High",
                DateTime.UtcNow);


        ticketReadRepository
            .GetByIdAsync(
                ticketId,
                Arg.Any<CancellationToken>())
            .Returns(ticket);


        var service =
            new SlaMonitoringService(
                slaReadRepository,
                slaRepository,
                ticketReadRepository,
                dispatcher);



        // Act

        await service.CheckAsync(
            CancellationToken.None);



        // Assert

        Assert.True(
            ticketSla.IsBreached);


        Assert.NotNull(
            ticketSla.BreachedDate);


        await slaRepository
            .Received(1)
            .UpdateAsync(
                ticketSla,
                Arg.Any<CancellationToken>());


        await dispatcher
            .Received(1)
            .Send<Guid>(
                Arg.Is<CreateNotificationCommand>(
                    command =>
                        command.OrganizationId == organizationId
                        &&
                        command.Type == "SLA_BREACH"),
                Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task Should_not_create_notification_when_ticket_not_found()
    {
        // Arrange

        var slaReadRepository =
            Substitute.For<ITicketSlaReadRepository>();

        var slaRepository =
            Substitute.For<ITicketSlaRepository>();

        var ticketReadRepository =
            Substitute.For<ITicketReadRepository>();

        var dispatcher =
            Substitute.For<IDispatcher>();


        var ticketSla =
            TicketSla.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                10,
                60);


        slaReadRepository
            .GetOverdueAsync(
                Arg.Any<DateTime>(),
                Arg.Any<CancellationToken>())
            .Returns(
                new List<TicketSla>
                {
                    ticketSla
                });


        ticketReadRepository
            .GetByIdAsync(
                Arg.Any<Guid>(),
                Arg.Any<CancellationToken>())
            .Returns((TicketDto?)null);


        var service =
            new SlaMonitoringService(
                slaReadRepository,
                slaRepository,
                ticketReadRepository,
                dispatcher);



        // Act

        await service.CheckAsync(
            CancellationToken.None);



        // Assert

        await slaRepository
            .Received(1)
            .UpdateAsync(
                ticketSla,
                Arg.Any<CancellationToken>());


        await dispatcher
            .DidNotReceive()
            .Send<Guid>(
                Arg.Any<CreateNotificationCommand>(),
                Arg.Any<CancellationToken>());
    }
}