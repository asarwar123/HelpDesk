using HelpDesk.api.Entities;

namespace HelpDesk.api.Services
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> getTicketsAsync();
        Task<Ticket?> getTicketAsync(Guid ticketId);
        Task<Ticket?> createTicketAsync(Ticket ticket);
        Task<Ticket?> updateTicketAsync(Guid ticketId);
        Task<bool> deleteTicketAysnc(Guid ticketId);
    }
}
