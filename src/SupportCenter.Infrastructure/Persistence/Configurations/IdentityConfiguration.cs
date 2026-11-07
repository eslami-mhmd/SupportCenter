using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SupportCenter.Infrastructure.Identity;
using System.Text;

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

        var serviceProvider = services.BuildServiceProvider();
        var jwtSettings =
            serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
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
                })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        jwtSettings.Secret)),
                            ClockSkew = TimeSpan.Zero
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
