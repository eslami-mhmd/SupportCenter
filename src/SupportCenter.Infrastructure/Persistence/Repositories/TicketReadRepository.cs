using Microsoft.EntityFrameworkCore;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Application.Features.Tickets.GetTicket;

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
}