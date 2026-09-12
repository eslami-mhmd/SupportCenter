using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Tickets.AssignTicket;
using SupportCenter.Domain.Tickets;
using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Auditing.CreateAuditEntry;

namespace SupportCenter.UnitTests.Tickets;

public class AssignTicketHandlerTests
{
    [Fact]
    public async Task Should_assign_ticket_to_user()
    {
        var repository =
            Substitute.For<ITicketWriteRepository>();


        var ticket =
            Ticket.Create(
                Guid.NewGuid(),
                "Printer issue",
                "Printer is not working");


        repository.GetAsync(
                ticket.Id,
                Arg.Any<CancellationToken>())
            .Returns(ticket);


        var dispatcher =
            Substitute.For<IDispatcher>();

        var handler =
            new AssignTicketCommandHandler(
                repository,
                dispatcher);


        var userId = Guid.NewGuid();


        var result =
            await handler.Handle(
                new AssignTicketCommand(
                    ticket.Id,
                    userId),
                CancellationToken.None);


        Assert.True(result);


        Assert.Equal(
            userId,
            ticket.AssignedUserId);


        await repository
            .Received(1)
            .UpdateAsync(
                ticket,
                Arg.Any<CancellationToken>());

        await dispatcher
            .Received(1)
            .Send<Guid>(
                Arg.Is<CreateAuditEntryCommand>(
                command =>
                command.Action == "TICKET_ASSIGNED"),
                Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task Should_return_false_when_ticket_not_found()
    {
        var repository =
            Substitute.For<ITicketWriteRepository>();


        repository.GetAsync(
                Arg.Any<Guid>(),
                Arg.Any<CancellationToken>())
            .Returns((Ticket?)null);

        var dispatcher =
            Substitute.For<IDispatcher>();

        var handler =
            new AssignTicketCommandHandler(
                repository,
                dispatcher);

        var result =
            await handler.Handle(
                new AssignTicketCommand(
                    Guid.NewGuid(),
                    Guid.NewGuid()),
                CancellationToken.None);


        Assert.False(result);


        await repository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<Ticket>(),
                Arg.Any<CancellationToken>());
    }
}