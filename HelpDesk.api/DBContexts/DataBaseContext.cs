using HelpDesk.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.api.DBContexts
{
    public class DataBaseContext: DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options):base(options)
        {

        }
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

    }
}
