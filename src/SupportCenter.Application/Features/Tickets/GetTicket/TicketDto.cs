namespace SupportCenter.Application.Features.Tickets.GetTicket;

public sealed record TicketDto(
    Guid Id,
    Guid OrganizationId,
    string Title,
    string Description,
    string Status,
    string Priority,
    DateTime CreatedDate);