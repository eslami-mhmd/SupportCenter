using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SupportCenter.Application.Abstractions.Identity;

namespace SupportCenter.Infrastructure.Identity;

public sealed class CurrentUserService 
    : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;


    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public Guid? UserId
    {
        get
        {
            var value =
                _httpContextAccessor
                    .HttpContext?
                    .User?
                    .FindFirstValue(
                        ClaimTypes.NameIdentifier);


            return Guid.TryParse(
                value,
                out var id)
                ? id
                : null;
        }
    }


    public bool IsAuthenticated =>
        _httpContextAccessor
            .HttpContext?
            .User?
            .Identity?
            .IsAuthenticated
            ?? false;
}