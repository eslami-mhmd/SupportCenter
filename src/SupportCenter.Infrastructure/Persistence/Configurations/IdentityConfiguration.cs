using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SupportCenter.Infrastructure.Identity;

namespace SupportCenter.Infrastructure.Persistence.Configurations;

public static class IdentityConfiguration
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services)
    {
        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}