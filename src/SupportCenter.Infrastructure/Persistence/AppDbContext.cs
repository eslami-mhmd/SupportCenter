using Microsoft.EntityFrameworkCore;
using SupportCenter.Domain.Organizations;
using SupportCenter.Domain.Tickets;
using SupportCenter.Domain.Sla;
using SupportCenter.Domain.Notifications;
using SupportCenter.Domain.Auditing;
using SupportCenter.Domain.Permissions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SupportCenter.Infrastructure.Identity;

namespace SupportCenter.Infrastructure.Persistence;


public class AppDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<SlaPolicy> SlaPolicies => Set<SlaPolicy>();
    public DbSet<TicketSla> TicketSlas => Set<TicketSla>();
    public DbSet<Notification> Notifications =>
    Set<Notification>();
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();
    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}