using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportCenter.Domain.Auditing;

namespace SupportCenter.Infrastructure.Persistence.Configurations;

public sealed class AuditEntryConfiguration
    : IEntityTypeConfiguration<AuditEntry>
{
    public void Configure(
        EntityTypeBuilder<AuditEntry> builder)
    {
        builder.ToTable("audit_entries");


        builder.HasKey(x => x.Id);


        builder.Property(x => x.Action)
            .HasMaxLength(100)
            .IsRequired();


        builder.Property(x => x.EntityName)
            .HasMaxLength(100)
            .IsRequired();
    }
}