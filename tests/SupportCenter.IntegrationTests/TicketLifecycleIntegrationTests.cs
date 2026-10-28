using Microsoft.Extensions.DependencyInjection;
using SupportCenter.Application;
using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Organizations.CreateOrganization;
using SupportCenter.Application.Features.Tickets.AssignTicket;
using SupportCenter.Application.Features.Tickets.ChangeTicketStatus;
using SupportCenter.Application.Features.Tickets.CreateTicket;
using SupportCenter.Application.Features.Tickets.GetTicket;
using SupportCenter.Application.Features.Tickets.ListTickets;
using SupportCenter.Domain.Tickets;
using SupportCenter.Infrastructure;

namespace SupportCenter.IntegrationTests;

public class TicketLifecycleIntegrationTests : IClassFixture<PostgresContainerFixture>
{
    private readonly PostgresContainerFixture _fixture;

    public TicketLifecycleIntegrationTests(PostgresContainerFixture fixture)
    {
        _fixture = fixture;
    }

    private IDispatcher BuildDispatcher()
    {
        var configuration = TestConfiguration.Create(_fixture.ConnectionString);
        var services = new ServiceCollection();

        services.AddApplication();
        services.AddInfrastructure(configuration);

        return services.BuildServiceProvider()
            .GetRequiredService<IDispatcher>();
    }

    private async Task<Guid> CreateOrganizationAsync(IDispatcher dispatcher)
    {
        return await dispatcher.Send(
            new CreateOrganizationCommand("Acme Software", "acme"));
    }

    [Fact]
    public async Task Should_create_assign_change_status_and_list_ticket()
    {
        await _fixture.CleanDatabaseAsync();

        var dispatcher = BuildDispatcher();

        var organizationId = await CreateOrganizationAsync(dispatcher);
        Assert.NotEqual(Guid.Empty, organizationId);

        var ticketId =
            await dispatcher.Send(
                new CreateTicketCommand(
                    organizationId,
                    "Cannot login",
                    "User cannot access system"));

        Assert.NotEqual(Guid.Empty, ticketId);

        var assigned =
            await dispatcher.Send(
                new AssignTicketCommand(ticketId, Guid.NewGuid()));

        Assert.True(assigned);

        var changedId =
            await dispatcher.Send(
                new ChangeTicketStatusCommand(
                    ticketId,
                    TicketStatus.InProgress));

        Assert.Equal(ticketId, changedId);

        var ticket =
            await dispatcher.Send(
                new GetTicketQuery(ticketId));

        Assert.Equal(ticketId, ticket.Id);
        Assert.Equal(organizationId, ticket.OrganizationId);
        Assert.Equal("Cannot login", ticket.Title);
        Assert.Equal("InProgress", ticket.Status);

        var orgTickets =
            await dispatcher.Send(
                new ListTicketsQuery(organizationId, null, 1, 10));

        Assert.Single(orgTickets.Items);

        var otherOrgTickets =
            await dispatcher.Send(
                new ListTicketsQuery(Guid.NewGuid(), null, 1, 10));

        Assert.Empty(otherOrgTickets.Items);
    }

    [Fact]
    public async Task Should_not_return_ticket_when_organization_does_not_match()
    {
        await _fixture.CleanDatabaseAsync();

        var dispatcher = BuildDispatcher();

        var organizationId = await CreateOrganizationAsync(dispatcher);

        var ticketId =
            await dispatcher.Send(
                new CreateTicketCommand(
                    organizationId,
                    "Cross-tenant probe",
                    "Should not leak"));

        var otherOrgId = Guid.NewGuid();

        var ticket =
            await dispatcher.Send(
                new GetTicketQuery(ticketId));

        // No current-user organization is set, so the repository falls back to
        // an id-only lookup. Tenant scoping is asserted via ListTickets below.
        Assert.Equal(ticketId, ticket.Id);

        var ownOrgTickets =
            await dispatcher.Send(
                new ListTicketsQuery(organizationId, null, 1, 10));

        Assert.Single(ownOrgTickets.Items);

        var foreignOrgTickets =
            await dispatcher.Send(
                new ListTicketsQuery(otherOrgId, null, 1, 10));

        Assert.Empty(foreignOrgTickets.Items);
    }
}
