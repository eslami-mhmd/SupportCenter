using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SupportCenter.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<
            IOrganizationRepository,
            InMemoryOrganizationRepository>();

        return services;
    }
}