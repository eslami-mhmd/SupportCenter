using NSubstitute;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Tickets.GetTicket;
using SupportCenter.Application.Features.Tickets.ListTickets;
using SupportCenter.Application.Common.Pagination;

namespace SupportCenter.UnitTests.Tickets;

public class ListTicketsHandlerTests
{
    [Fact]
    public async Task Should_return_tickets()
    {
        // Arrange
        var repository = Substitute.For<ITicketReadRepository>();

        var organizationId = Guid.NewGuid();

        var query = new ListTicketsQuery(
            organizationId,
            null,
            1,
            10);


    var tickets = new List<TicketDto>
    {
        new(
            Guid.NewGuid(),
            organizationId,
            "First ticket",
            "Description 1",
            "Open",
            "High",
            DateTime.UtcNow),

        new(
            Guid.NewGuid(),
            organizationId,
            "Second ticket",
            "Description 2",
            "Open",
            "Medium",
            DateTime.UtcNow)
    };


        repository
            .ListAsync(
                query,
                Arg.Any<CancellationToken>())
            .Returns(
                new PagedResult<TicketDto>(
                    tickets,
                    tickets.Count,
                    1,
                    10));


        var handler = new ListTicketsQueryHandler(repository);


        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);


        // Assert
        Assert.Equal(2, result.Items.Count);

        await repository
            .Received(1)
            .ListAsync(
                query,
                Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task Should_return_empty_result_when_no_tickets_exist()
    {
        // Arrange
        var repository = Substitute.For<ITicketReadRepository>();

        var query = new ListTicketsQuery(
            Guid.NewGuid(),
            null,
            1,
            10);


        repository
            .ListAsync(
                query,
                Arg.Any<CancellationToken>())
            .Returns(
                new PagedResult<TicketDto>(
                    new List<TicketDto>(),
                    0,
                    1,
                    10));


        var handler = new ListTicketsQueryHandler(repository);


        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);


        // Assert
        Assert.Empty(result.Items);

        await repository
            .Received(1)
            .ListAsync(
                query,
                Arg.Any<CancellationToken>());
    }
}