using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportCenter.Domain.Notifications;

namespace SupportCenter.Infrastructure.Persistence.Configurations;

public sealed class NotificationConfiguration
    : IEntityTypeConfiguration<Notification>
{
    public void Configure(
        EntityTypeBuilder<Notification> builder)
    {
        builder
            .HasKey(x => x.Id);


        builder
            .Property(x => x.Type)
            .HasMaxLength(100)
            .IsRequired();


        builder
            .Property(x => x.Message)
            .HasMaxLength(1000)
            .IsRequired();


        builder
            .HasIndex(x => x.OrganizationId);
    }
}