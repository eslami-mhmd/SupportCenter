using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Tickets.GetTicket;

public sealed record GetTicketQuery(Guid Id)
    : IQuery<TicketDto?>;