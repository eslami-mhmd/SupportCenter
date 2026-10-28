using Microsoft.EntityFrameworkCore;
using SupportCenter.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace SupportCenter.IntegrationTests;

public sealed class PostgresContainerFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container =
        new PostgreSqlBuilder("postgres:17")
            .WithDatabase("supportcenter")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var options =
            new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

        await using var context = new AppDbContext(options);
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public async Task CleanDatabaseAsync()
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

        await using var context = new AppDbContext(options);

        await context.Database.ExecuteSqlRawAsync("DELETE FROM \"TicketSlas\"");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM \"Notifications\"");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM \"audit_entries\"");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM \"Tickets\"");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM \"SlaPolicies\"");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM \"Organizations\"");
    }
}
