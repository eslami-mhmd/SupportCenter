using Microsoft.Extensions.DependencyInjection;
using SupportCenter.Application;
using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Organizations.CreateOrganization;
using SupportCenter.Application.Features.Sla.CreateSlaPolicy;
using SupportCenter.Domain.Tickets;
using SupportCenter.Infrastructure;

namespace SupportCenter.IntegrationTests;

public class SlaPolicyIntegrationTests : IClassFixture<PostgresContainerFixture>
{
    private readonly PostgresContainerFixture _fixture;

    public SlaPolicyIntegrationTests(PostgresContainerFixture fixture)
    {
        _fixture = fixture;
    }

    private (IDispatcher Dispatcher, ISlaPolicyRepository SlaPolicyRepository)
        BuildServices()
    {
        var configuration = TestConfiguration.Create(_fixture.ConnectionString);
        var services = new ServiceCollection();

        services.AddApplication();
        services.AddInfrastructure(configuration);

        var provider = services.BuildServiceProvider();

        return (
            provider.GetRequiredService<IDispatcher>(),
            provider.GetRequiredService<ISlaPolicyRepository>());
    }

    [Fact]
    public async Task Should_create_sla_policy_for_organization()
    {
        await _fixture.CleanDatabaseAsync();

        var (dispatcher, slaPolicyRepository) = BuildServices();

        var organizationId =
            await dispatcher.Send(
                new CreateOrganizationCommand("Acme Software", "acme"));

        Assert.NotEqual(Guid.Empty, organizationId);

        var policyId =
            await dispatcher.Send(
                new CreateSlaPolicyCommand(
                    organizationId,
                    TicketPriority.High,
                    30,
                    240));

        Assert.NotEqual(Guid.Empty, policyId);

        var policy =
            await slaPolicyRepository.GetAsync(
                organizationId,
                TicketPriority.High,
                CancellationToken.None);

        Assert.NotNull(policy);
        Assert.Equal(policyId, policy.Id);
        Assert.Equal(organizationId, policy.OrganizationId);
        Assert.Equal(30, policy.ResponseTimeMinutes);
        Assert.Equal(240, policy.ResolutionTimeMinutes);
    }
}
