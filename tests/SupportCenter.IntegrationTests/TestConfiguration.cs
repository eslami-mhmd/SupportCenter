using Microsoft.Extensions.Configuration;

namespace SupportCenter.IntegrationTests;

public static class TestConfiguration
{
    public static IConfiguration Create(string connectionString)
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
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
