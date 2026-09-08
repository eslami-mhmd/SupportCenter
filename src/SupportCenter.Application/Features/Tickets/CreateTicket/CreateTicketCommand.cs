using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Tickets.CreateTicket;

public sealed record CreateTicketCommand(
    Guid OrganizationId,
    string Title,
    string Description)
    : ICommand<Guid>;