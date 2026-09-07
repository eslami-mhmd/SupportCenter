using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SupportCenter.Application;
using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Organizations.CreateOrganization;
using SupportCenter.Infrastructure;

namespace SupportCenter.UnitTests.Organizations;

public class CreateOrganizationTests
{
    [Fact]
    public async Task Should_create_organization()
    {
        var configuration =
            new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                {
                    "ConnectionStrings:Database",
                    "Host=localhost;Port=5432;Database=supportcenter;Username=postgres;Password=postgres"
                }
                    })
                .Build();

        var services = new ServiceCollection();

        services.AddApplication();
        Console.WriteLine(configuration.GetConnectionString("Databse"));
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