using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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

        services
            .AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.Cookie.Name = "SupportCenter.Auth";
                    options.LoginPath = "/account/login";

                    options.Events.OnRedirectToLogin =
                        context =>
                        {
                            if (IsApiRequest(context.Request))
                            {
                                context.Response.StatusCode =
                                    StatusCodes.Status401Unauthorized;
                            }

                            return Task.CompletedTask;
                        };

                    options.Events.OnRedirectToAccessDenied =
                        context =>
                        {
                            if (IsApiRequest(context.Request))
                            {
                                context.Response.StatusCode =
                                    StatusCodes.Status403Forbidden;
                            }

                            return Task.CompletedTask;
                        };
                });

        return services;
    }

    private static bool IsApiRequest(HttpRequest request)
    {
        if (request.Path.StartsWithSegments("/api"))
        {
            return true;
        }

        return request.Headers.Accept
            .Any(accept => accept?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true);
    }
}