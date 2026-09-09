using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportCenter.Domain.Sla;

namespace SupportCenter.Infrastructure.Persistence.Configurations;

public sealed class SlaPolicyConfiguration
    : IEntityTypeConfiguration<SlaPolicy>
{
    public void Configure(
        EntityTypeBuilder<SlaPolicy> builder)
    {
        builder.ToTable("SlaPolicies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Priority)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.ResponseTimeMinutes)
            .IsRequired();

        builder.Property(x => x.ResolutionTimeMinutes)
            .IsRequired();

        builder.HasIndex(
                x => new
                {
                    x.OrganizationId,
                    x.Priority
                })
            .IsUnique();
    }
}