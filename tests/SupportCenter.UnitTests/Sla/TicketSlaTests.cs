using SupportCenter.Domain.Exceptions;
using SupportCenter.Domain.Sla;

namespace SupportCenter.UnitTests.Sla;

public class TicketSlaTests
{
    [Fact]
    public void Should_create_ticket_sla()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var policyId = Guid.NewGuid();


        // Act
        var ticketSla = TicketSla.Create(
            ticketId,
            policyId,
            30,
            480);


        // Assert
        Assert.NotEqual(
            Guid.Empty,
            ticketSla.Id);

        Assert.Equal(
            ticketId,
            ticketSla.TicketId);

        Assert.Equal(
            policyId,
            ticketSla.SlaPolicyId);

        Assert.False(
            ticketSla.IsBreached);


        Assert.True(
            ticketSla.ResponseDeadline > DateTime.UtcNow);


        Assert.True(
            ticketSla.ResolutionDeadline >
            ticketSla.ResponseDeadline);
    }


    [Fact]
    public void Should_not_create_without_ticket()
    {
        var exception = Assert.Throws<DomainException>(
            () =>
                TicketSla.Create(
                    Guid.Empty,
                    Guid.NewGuid(),
                    30,
                    480));


        Assert.Equal(
            "Ticket is required.",
            exception.Message);
    }



    [Fact]
    public void Should_not_create_without_policy()
    {
        var exception = Assert.Throws<DomainException>(
            () =>
                TicketSla.Create(
                    Guid.NewGuid(),
                    Guid.Empty,
                    30,
                    480));


        Assert.Equal(
            "SLA policy is required.",
            exception.Message);
    }



    [Fact]
    public void Should_not_create_with_invalid_response_time()
    {
        var exception = Assert.Throws<DomainException>(
            () =>
                TicketSla.Create(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    0,
                    480));


        Assert.Equal(
            "Response time must be positive.",
            exception.Message);
    }



    [Fact]
    public void Should_mark_as_breached()
    {
        var ticketSla = TicketSla.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            30,
            480);


        ticketSla.MarkAsBreached();


        Assert.True(
            ticketSla.IsBreached);


        Assert.NotNull(
            ticketSla.BreachedDate);
    }

    [Fact]
    public void Should_not_change_breach_date_when_already_breached()
    {
        var ticketSla = TicketSla.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            30,
            480);


        ticketSla.MarkAsBreached();

        var firstDate =
            ticketSla.BreachedDate;


        ticketSla.MarkAsBreached();


        Assert.Equal(
            firstDate,
            ticketSla.BreachedDate);
    }
}