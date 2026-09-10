using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportCenter.Domain.Sla;

namespace SupportCenter.Infrastructure.Persistence.Configurations;

public sealed class TicketSlaConfiguration
    : IEntityTypeConfiguration<TicketSla>
{
    public void Configure(
        EntityTypeBuilder<TicketSla> builder)
    {
        builder.ToTable("TicketSlas");


        builder.HasKey(x => x.Id);


        builder.Property(x => x.ResponseDeadline)
            .IsRequired();


        builder.Property(x => x.ResolutionDeadline)
            .IsRequired();


        builder.Property(x => x.IsBreached)
            .IsRequired();
    }
}