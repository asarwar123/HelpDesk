using HelpDesk.api.DBContexts;
using HelpDesk.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.api.Services
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DataBaseContext _context;

        public TicketRepository(DataBaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> createTicketAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            return await _context.SaveChangesAsync();
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
            Ticket? ticket = await _context.Tickets.FindAsync(ticketId);
            return ticket;
        }

        public async Task<(IEnumerable<Ticket>,PaginationMetadata)> getTicketsAsync(int pageSize, int pageNumber)
        {
            //IEnumerable<Ticket> tickets = await _context.Tickets.OrderBy(t => t.CreatedAt).ToListAsync();
            var collection = _context.Tickets as IQueryable<Ticket>;

            var TotalCount = await collection.CountAsync();

            var paginationMetaData = new PaginationMetadata(TotalCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();

            return (collectionToReturn, paginationMetaData);
        }

        public async Task<(IEnumerable<Ticket>, PaginationMetadata)> getTicketsAsync(string? filterText, string? queryString,int pageSize,int pageNumber)
        {
            if (String.IsNullOrEmpty(filterText)
                && String.IsNullOrEmpty(queryString))
            {
                return await getTicketsAsync(pageSize,pageNumber);
            }

            var collection = _context.Tickets as IQueryable<Ticket>;

            if (!string.IsNullOrEmpty(filterText))
            {
                filterText = filterText.Trim();
                collection = collection.Where(c => c.subject == filterText);
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                queryString = queryString.Trim();
                collection = collection.Where(c => c.message.Contains(queryString) ||
                                                    c.subject.Contains(queryString));
            }

            var TotalCount = await collection.CountAsync();

            var paginationMetaData = new PaginationMetadata(TotalCount,pageSize,pageNumber);

            var collectionToReturn = await collection
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();

            return (collectionToReturn, paginationMetaData);

            //return await collection.OrderBy(t => t.CreatedAt).ToListAsync();
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
