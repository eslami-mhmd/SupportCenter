using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Infrastructure.Persistence;
using SupportCenter.Infrastructure.Persistence.Repositories;

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


        return services;
    }
}