using Microsoft.EntityFrameworkCore;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Tickets.GetTicket;
using SupportCenter.Application.Features.Tickets.ListTickets;
using SupportCenter.Application.Common.Pagination;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class TicketReadRepository
    : ITicketReadRepository
{
    private readonly AppDbContext _context;


    public TicketReadRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task<TicketDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Tickets
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TicketDto(
                x.Id,
                x.OrganizationId,
                x.Title,
                x.Description,
                x.Status.ToString(),
                x.Priority.ToString(),
                x.CreatedDate))
            .FirstOrDefaultAsync(
                cancellationToken);
    }

    public async Task<PagedResult<TicketDto>> ListAsync(
    ListTicketsQuery query,
    CancellationToken cancellationToken)
    {
        var tickets = _context.Tickets
            .AsNoTracking()
            .Where(x =>
                x.OrganizationId == query.OrganizationId);


        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            tickets = tickets.Where(x =>
                x.Status.ToString() == query.Status);
        }


        var totalCount = await tickets.CountAsync(
            cancellationToken);


        var items = await tickets
            .OrderByDescending(x => x.CreatedDate)
            .Skip(
                (query.PageNumber - 1) *
                query.PageSize)
            .Take(query.PageSize)
            .Select(x => new TicketDto(
                x.Id,
                x.OrganizationId,
                x.Title,
                x.Description,
                x.Status.ToString(),
                x.Priority.ToString(),
                x.CreatedDate))
            .ToListAsync(
                cancellationToken);


        return new PagedResult<TicketDto>(
            items,
            query.PageNumber,
            query.PageSize,
            totalCount);
    }
}