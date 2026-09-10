using Microsoft.EntityFrameworkCore;
using SupportCenter.Domain.Organizations;
using SupportCenter.Domain.Tickets;
using SupportCenter.Domain.Users;
using SupportCenter.Domain.Sla;

namespace SupportCenter.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<User> Users => Set<User>();
    public DbSet<SlaPolicy> SlaPolicies => Set<SlaPolicy>();
    public DbSet<TicketSla> TicketSlas => Set<TicketSla>();
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}