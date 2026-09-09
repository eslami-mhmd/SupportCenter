using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Exceptions;
using SupportCenter.Domain.Exceptions;
using SupportCenter.Application.Features.Tickets.ChangeTicketStatus;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.UnitTests.Tickets;

public class ChangeTicketStatusHandlerTests
{
    [Fact]
    public async Task Should_change_ticket_status()
    {
        // Arrange

        var repository =
            Substitute.For<ITicketWriteRepository>();

        var ticket =
            Ticket.Create(
                Guid.NewGuid(),
                "Cannot login",
                "Customer cannot access account");


        repository
            .GetAsync(
                ticket.Id,
                Arg.Any<CancellationToken>())
            .Returns(ticket);


        var handler =
            new ChangeTicketStatusCommandHandler(
                repository);


        var command =
            new ChangeTicketStatusCommand(
                ticket.Id,
                TicketStatus.InProgress);


        // Act

        var result =
            await handler.Handle(
                command,
                CancellationToken.None);


        // Assert

        Assert.Equal(
            ticket.Id,
            result);

        Assert.Equal(
            TicketStatus.InProgress,
            ticket.Status);


        await repository
            .Received(1)
            .UpdateAsync(
                ticket,
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Should_not_close_open_ticket_directly()
    {
        var repository =
            Substitute.For<ITicketWriteRepository>();

        var ticket =
            Ticket.Create(
                Guid.NewGuid(),
                "Cannot login",
                "Customer cannot access account");


        repository
            .GetAsync(
                ticket.Id,
                Arg.Any<CancellationToken>())
            .Returns(ticket);


        var handler =
            new ChangeTicketStatusCommandHandler(
                repository);


        var command =
            new ChangeTicketStatusCommand(
                ticket.Id,
                TicketStatus.Closed);


        await Assert.ThrowsAsync<DomainException>(
            () =>
                handler.Handle(
                    command,
                    CancellationToken.None));
    }
}