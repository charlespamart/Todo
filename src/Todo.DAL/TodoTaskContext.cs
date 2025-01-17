﻿using Microsoft.EntityFrameworkCore;
using Todo.DAL.Models;

namespace Todo.DAL
{
    public class TodoTaskContext : DbContext
    {
        public TodoTaskContext(DbContextOptions<TodoTaskContext> options)
            : base(options)
        {
        }

        public DbSet<TodoTaskData> TodoTasks { get; set; }
    }
}
