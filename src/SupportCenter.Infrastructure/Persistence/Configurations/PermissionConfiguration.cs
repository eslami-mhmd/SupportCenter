using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportCenter.Domain.Permissions;

namespace SupportCenter.Infrastructure.Persistence.Configurations;

public sealed class PermissionConfiguration
    : IEntityTypeConfiguration<Permission>
{
    public void Configure(
        EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");


        builder.HasKey(x => x.Id);


        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();
    }
}