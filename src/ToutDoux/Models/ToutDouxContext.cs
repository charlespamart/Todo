using Microsoft.EntityFrameworkCore;

namespace ToutDoux.Models
{
    public class ToutDouxContext : DbContext
    {
        public ToutDouxContext(DbContextOptions<ToutDouxContext> options)
            : base(options)
        {
        }

        public DbSet<ToutDouxTask> ToutDouxTasks { get; set; }
    }
}
