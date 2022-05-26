using HelpDesk.api.Entities;

namespace HelpDesk.api.Services
{
    public interface ITicketRepository
    {
        Task<(IEnumerable<Ticket>, PaginationMetadata)> getTicketsAsync(int pageSize, int pageNumber);
        Task<Ticket?> getTicketAsync(Guid ticketId);
        Task<(IEnumerable<Ticket>, PaginationMetadata)> getTicketsAsync(string? filterText,string? queryString,int pageSize,int pageNumber);
        Task<int> createTicketAsync(Ticket ticket);
        Task<Ticket?> updateTicketAsync(Guid ticketId);
        Task<bool> deleteTicketAysnc(Guid ticketId);
    }
}
