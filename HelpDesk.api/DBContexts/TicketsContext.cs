using HelpDesk.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.api.DBContexts
{
    public class TicketContext: DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options):base(options)
        {

        }
        public DbSet<Ticket> Tickets { get; set; } = null!;
    }
}
