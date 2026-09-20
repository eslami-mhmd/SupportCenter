using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Infrastructure.Persistence;
using SupportCenter.Infrastructure.Persistence.Repositories;
using SupportCenter.Application.Abstractions.Sla;
using SupportCenter.Application.Features.Sla;
using SupportCenter.Infrastructure.BackgroundJobs;
using SupportCenter.Infrastructure.Identity;
using SupportCenter.Infrastructure.Persistence.Configurations;
using SupportCenter.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Authorization;
using SupportCenter.Application.Security;

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
        services.AddScoped<
            INotificationRepository,
            NotificationRepository>();
        services.AddScoped<
        INotificationReadRepository,
        NotificationReadRepository>();
        services.AddScoped<
            IAuditRepository,
            AuditRepository>();
        services.AddIdentityServices();
        services.AddScoped<RbacSeeder>();

        services.AddHttpContextAccessor();

        services.AddScoped<
            ICurrentUserService,
            CurrentUserService>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                Permissions.TicketsAssign,
                policy =>
                {
                    policy.Requirements.Add(
                        new PermissionRequirement(
                            Permissions.TicketsAssign));
                });

            options.AddPolicy(
                Permissions.TicketsChangeStatus,
                policy =>
                {
                    policy.Requirements.Add(
                        new PermissionRequirement(
                            Permissions.TicketsChangeStatus));
                });
        });


        services.AddSingleton<IAuthorizationHandler,
            PermissionAuthorizationHandler>();

        services.AddScoped<
            IUserRoleService,
            UserRoleService>();

        services.AddHostedService<NotificationProcessingWorker>();
        services.AddHostedService<SlaMonitoringWorker>();

        return services;
    }
}