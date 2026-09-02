using Microsoft.Extensions.DependencyInjection;
using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();

        return services;
    }
}