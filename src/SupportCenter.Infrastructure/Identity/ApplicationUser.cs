using Microsoft.AspNetCore.Identity;

namespace SupportCenter.Infrastructure.Identity;

public sealed class ApplicationUser
    : IdentityUser<Guid>
{
    public Guid? OrganizationId { get; set; }
}