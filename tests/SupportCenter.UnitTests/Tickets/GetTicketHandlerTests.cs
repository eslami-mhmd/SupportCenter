using NSubstitute;
using SupportCenter.Application.Abstractions.Identity;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Tickets.GetTicket;
using SupportCenter.Application.Exceptions;

namespace SupportCenter.UnitTests.Tickets;

public class GetTicketHandlerTests
{
    [Fact]
    public async Task Should_return_ticket()
    {
        // Arrange
        var repository = Substitute.For<ITicketReadRepository>();

        var ticketId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var ticket =
            new TicketDto(
                ticketId,
                organizationId,
                "Cannot login",
                "User cannot access account",
                "Open",
                "High",
                DateTime.UtcNow);


        repository
            .GetByIdAsync(
                ticketId,
                organizationId,
                Arg.Any<CancellationToken>())
            .Returns(ticket);


        var currentUser =
            Substitute.For<ICurrentUserService>();

        currentUser
            .OrganizationId
            .Returns(organizationId);


        var handler =
            new GetTicketQueryHandler(
                repository,
                currentUser);


        // Act
        var result = await handler.Handle(
            new GetTicketQuery(ticketId),
            CancellationToken.None);


        // Assert
        Assert.Equal(ticketId, result.Id);

        await repository
            .Received(1)
            .GetByIdAsync(
                ticketId,
                Arg.Any<Guid?>(),
                Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task Should_throw_when_ticket_not_found()
    {
        // Arrange
        var repository = Substitute.For<ITicketReadRepository>();

        var ticketId = Guid.NewGuid();

        repository
            .GetByIdAsync(
                ticketId,
                Arg.Any<Guid?>(),
                Arg.Any<CancellationToken>())
            .Returns((TicketDto?)null);


        var currentUser =
            Substitute.For<ICurrentUserService>();

        currentUser
            .OrganizationId
            .Returns((Guid?)null);


        var handler =
            new GetTicketQueryHandler(
                repository,
                currentUser);


        // Act + Assert

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(
                new GetTicketQuery(ticketId),
                CancellationToken.None));
    }
}