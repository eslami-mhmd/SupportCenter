using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Common.Pagination;
using SupportCenter.Application.Features.Tickets.GetTicket;

namespace SupportCenter.Application.Features.Tickets.ListTickets;

public sealed record ListTicketsQuery(
    Guid OrganizationId,
    string? Status,
    int PageNumber = 1,
    int PageSize = 10)
    : IQuery<PagedResult<TicketDto>>;