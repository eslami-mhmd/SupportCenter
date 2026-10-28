using Microsoft.Extensions.DependencyInjection;
using SupportCenter.Application;
using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Organizations.CreateOrganization;
using SupportCenter.Infrastructure;

namespace SupportCenter.IntegrationTests.Organizations;

public class CreateOrganizationTests : IClassFixture<PostgresContainerFixture>
{
    private readonly PostgresContainerFixture _fixture;

    public CreateOrganizationTests(PostgresContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_create_organization()
    {
        await _fixture.CleanDatabaseAsync();

        var configuration = TestConfiguration.Create(_fixture.ConnectionString);
        var services = new ServiceCollection();

        services.AddApplication();
        services.AddInfrastructure(configuration);

        var provider = services.BuildServiceProvider();

        var dispatcher =
            provider.GetRequiredService<IDispatcher>();

        var organizationId =
            await dispatcher.Send(
                new CreateOrganizationCommand(
                    "Acme Software",
                    "acme"));

        Assert.NotEqual(
            Guid.Empty,
            organizationId);
    }
}
