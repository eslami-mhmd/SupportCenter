using SupportCenter.Application;
using SupportCenter.Infrastructure;
using SupportCenter.Api.Endpoints.Organizations;
using SupportCenter.Api.Endpoints.Tickets;
using SupportCenter.Api.Endpoints.Users;
using SupportCenter.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapCreateOrganization();
app.MapCreateTicket();
app.MapGetTicket();
app.MapListTickets();
app.MapChangeTicketStatus();
app.MapCreateUser();

app.Run();