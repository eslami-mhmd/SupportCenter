using Microsoft.EntityFrameworkCore;
using SupportCenter.Domain.Organizations;
using SupportCenter.Domain.Tickets;

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
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}