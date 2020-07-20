using Microsoft.EntityFrameworkCore;
using Todo.Domain.Models;

namespace Todo.DAL
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoTaskData> TodoTasks { get; set; }
    }
}
