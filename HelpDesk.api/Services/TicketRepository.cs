using HelpDesk.api.DBContexts;
using HelpDesk.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.api.Services
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketContext _context;

        public TicketRepository(TicketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Ticket?> createTicketAsync(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> deleteTicketAysnc(Guid ticketId)
        {
            Ticket? ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.id == ticketId);

            if (ticket != null)
            {
                _context.Remove(ticket);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<Ticket?> getTicketAsync(Guid ticketId)
        {
            Ticket? ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.id == ticketId);
            return ticket;
        }

        public async Task<IEnumerable<Ticket>> getTicketsAsync()
        {
            IEnumerable<Ticket> tickets = await _context.Tickets.OrderBy(t => t.CreatedAt).ToListAsync();
            return tickets;
        }

        public async Task<Ticket?> updateTicketAsync(Guid ticketId)
        {
            Ticket? ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.id == ticketId);

            if (ticket != null)
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();

                return ticket;
            }

            return null;
        }
    }
}
