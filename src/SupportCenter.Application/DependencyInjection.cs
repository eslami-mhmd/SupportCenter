using Microsoft.Extensions.DependencyInjection;
using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(typeof(DependencyInjection).Assembly)

            .AddClasses(classes => classes
                .AssignableTo(typeof(ICommandHandler<,>)))

            .AsImplementedInterfaces()
            .WithScopedLifetime());


        services.Scan(scan => scan
            .FromAssemblies(typeof(DependencyInjection).Assembly)

            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))

            .AsImplementedInterfaces()
            .WithScopedLifetime());


        services.AddScoped<IDispatcher, Dispatcher>();

        return services;
    }
}