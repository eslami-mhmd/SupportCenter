using Microsoft.Extensions.Configuration;

namespace SupportCenter.IntegrationTests;

public static class TestConfiguration
{
    public static IConfiguration Create(string connectionString)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {
                    "ConnectionStrings:Database",
                    connectionString
                }
            })
            .Build();
    }
}
