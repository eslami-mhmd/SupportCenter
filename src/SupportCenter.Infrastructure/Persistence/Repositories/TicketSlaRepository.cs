using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Sla;

namespace SupportCenter.Infrastructure.Persistence.Repositories;

public sealed class TicketSlaRepository
    : ITicketSlaRepository
{
    private readonly AppDbContext _context;


    public TicketSlaRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(
        TicketSla ticketSla,
        CancellationToken cancellationToken)
    {
        await _context.TicketSlas.AddAsync(
            ticketSla,
            cancellationToken);


        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task UpdateAsync(
    TicketSla ticketSla,
    CancellationToken cancellationToken)
    {
        _context.TicketSlas.Update(ticketSla);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}