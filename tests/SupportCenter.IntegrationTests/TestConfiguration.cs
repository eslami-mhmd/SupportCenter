using Microsoft.Extensions.Configuration;

namespace SupportCenter.IntegrationTests;

public static class TestConfiguration
{
    public static IConfiguration Create()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {
                    "ConnectionStrings:Database",
                    "Host=localhost;Port=5432;Database=supportcenter;Username=postgres;Password=postgres"
                }
            })
            .Build();
    }
}