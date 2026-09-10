using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Infrastructure.Persistence;
using SupportCenter.Infrastructure.Persistence.Repositories;
using SupportCenter.Application.Abstractions.Sla;
using SupportCenter.Application.Features.Sla;

namespace SupportCenter.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(
            options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("Database"));
            });


        services.AddScoped<
            IOrganizationRepository,
            OrganizationRepository>();
        services.AddScoped<
            ITicketWriteRepository,
            TicketRepository>();
        services.AddScoped<
            ITicketReadRepository,
            TicketReadRepository>();
        services.AddScoped<
            IUserRepository,
            UserRepository>();
        services.AddScoped<
            ISlaPolicyRepository,
            SlaPolicyRepository>();
        services.AddScoped<
            ITicketSlaRepository,
            TicketSlaRepository>();
        services.AddScoped<
            ITicketSlaReadRepository,
            TicketSlaReadRepository>();
        services.AddScoped<
            ISlaMonitoringService,
             SlaMonitoringService>();

        return services;
    }
}