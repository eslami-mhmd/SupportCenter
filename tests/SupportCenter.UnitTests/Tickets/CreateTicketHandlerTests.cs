using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Tickets.CreateTicket;
using SupportCenter.Domain.Sla;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.UnitTests.Tickets;

public class CreateTicketHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Ticket()
    {
        // Arrange
        var ticketRepository =
            Substitute.For<ITicketWriteRepository>();

        var slaPolicyRepository =
            Substitute.For<ISlaPolicyRepository>();

        var ticketSlaRepository =
            Substitute.For<ITicketSlaRepository>();


        var handler = new CreateTicketCommandHandler(
            ticketRepository,
            slaPolicyRepository,
            ticketSlaRepository);


        var organizationId = Guid.NewGuid();


        var command = new CreateTicketCommand(
            organizationId,
            "Cannot login",
            "User cannot access the application after password reset");


        Ticket? createdTicket = null;


        ticketRepository
            .AddAsync(
                Arg.Do<Ticket>(
                    ticket => createdTicket = ticket),
                Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);



        var slaPolicy = SlaPolicy.Create(
            organizationId,
            TicketPriority.Medium,
            30,
            240);



        slaPolicyRepository
            .GetAsync(
                organizationId,
                TicketPriority.Medium,
                Arg.Any<CancellationToken>())
            .Returns(slaPolicy);



        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);



        // Assert

        Assert.NotEqual(
            Guid.Empty,
            result);


        Assert.NotNull(
            createdTicket);


        Assert.Equal(
            organizationId,
            createdTicket.OrganizationId);


        Assert.Equal(
            "Cannot login",
            createdTicket.Title);


        Assert.Equal(
            "User cannot access the application after password reset",
            createdTicket.Description);


        Assert.Equal(
            TicketStatus.Open,
            createdTicket.Status);


        Assert.Equal(
            TicketPriority.Medium,
            createdTicket.Priority);



        await ticketRepository
            .Received(1)
            .AddAsync(
                Arg.Any<Ticket>(),
                Arg.Any<CancellationToken>());



        await ticketSlaRepository
            .Received(1)
            .AddAsync(
                Arg.Any<TicketSla>(),
                Arg.Any<CancellationToken>());
    }
}