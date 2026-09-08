using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Tickets.GetTicket;

namespace SupportCenter.UnitTests.Tickets;

public class GetTicketHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Ticket_When_Exists()
    {
        // Arrange
        var repository = Substitute.For<ITicketReadRepository>();

        var handler = new GetTicketQueryHandler(repository);

        var ticketId = Guid.NewGuid();

        var expectedTicket = new TicketDto(
            ticketId,
            Guid.NewGuid(),
            "Cannot login",
            "User cannot access the application",
            "Open",
            "Medium",
            DateTime.UtcNow);


        repository
            .GetByIdAsync(
                ticketId,
                Arg.Any<CancellationToken>())
            .Returns(expectedTicket);


        var query = new GetTicketQuery(ticketId);


        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);


        // Assert
        Assert.NotNull(result);

        Assert.Equal(
            expectedTicket.Id,
            result.Id);

        Assert.Equal(
            expectedTicket.Title,
            result.Title);

        Assert.Equal(
            expectedTicket.Description,
            result.Description);


        await repository
            .Received(1)
            .GetByIdAsync(
                ticketId,
                Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task Handle_Should_Return_Null_When_Ticket_Does_Not_Exist()
    {
        // Arrange
        var repository = Substitute.For<ITicketReadRepository>();

        var handler = new GetTicketQueryHandler(repository);

        var ticketId = Guid.NewGuid();


        repository
            .GetByIdAsync(
                ticketId,
                Arg.Any<CancellationToken>())
            .Returns((TicketDto?)null);


        var query = new GetTicketQuery(ticketId);


        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);


        // Assert
        Assert.Null(result);


        await repository
            .Received(1)
            .GetByIdAsync(
                ticketId,
                Arg.Any<CancellationToken>());
    }
}