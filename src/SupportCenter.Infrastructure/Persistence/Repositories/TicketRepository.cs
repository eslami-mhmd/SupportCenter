using Microsoft.EntityFrameworkCore;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Tickets;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class TicketRepository : ITicketWriteRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Ticket?> GetAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Tickets
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
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


    public async Task UpdateAsync(
        Ticket ticket,
        CancellationToken cancellationToken)
    {
        _context.Tickets.Update(ticket);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}