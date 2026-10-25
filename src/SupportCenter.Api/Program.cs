using Serilog;
using Serilog.Events;
using SupportCenter.Application;
using SupportCenter.Infrastructure;
using SupportCenter.Api.Endpoints.Organizations;
using SupportCenter.Api.Endpoints.Tickets;
using SupportCenter.Api.Endpoints.Sla;
using SupportCenter.Api.Middleware;
using SupportCenter.Infrastructure.Identity;
using SupportCenter.Api.Endpoints.Users;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services
    .AddApplication();

builder.Services
    .AddInfrastructure(
        builder.Configuration);

builder.Services
    .AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder =
        scope.ServiceProvider
            .GetRequiredService<RbacSeeder>();

    await seeder.SeedAsync();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapCreateOrganization();
app.MapCreateTicket();
app.MapGetTicket();
app.MapListTickets();
app.MapChangeTicketStatus();
app.MapAssignTicket();
app.MapCreateSlaPolicy();
app.MapAssignRoleEndpoint();
app.MapCreateUserEndpoint();
app.MapLoginEndpoint();

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}