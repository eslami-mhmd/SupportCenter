using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Application.Features.Sla.CreateSlaPolicy;

public sealed record CreateSlaPolicyCommand(
    Guid OrganizationId,
    TicketPriority Priority,
    int ResponseTimeMinutes,
    int ResolutionTimeMinutes)
    : ICommand<Guid>;