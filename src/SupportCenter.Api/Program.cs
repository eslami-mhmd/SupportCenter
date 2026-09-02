using SupportCenter.Application;
using SupportCenter.Infrastructure;
using SupportCenter.Api.Endpoints.Organizations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication();

builder.Services
    .AddInfrastructure(
        builder.Configuration);

builder.Services
    .AddOpenApi();

var app = builder.Build();

app.MapCreateOrganization();

app.Run();