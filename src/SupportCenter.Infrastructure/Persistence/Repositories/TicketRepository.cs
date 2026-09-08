using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class TicketRepository 
    : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Ticket ticket,
        CancellationToken cancellationToken)
    {
        await _context.Tickets.AddAsync(
            ticket,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}