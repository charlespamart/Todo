using System;
using System.Diagnostics.CodeAnalysis;

namespace Todo.Domain.Models
{
    public class TodoTask
    {
        public Guid Id { get; }
        public string Title { get; }
        public bool Completed { get; }
        public int Order { get; }

        public TodoTask(Guid id, string title, bool completed, int order)
        {
            Id = id;
            Title = title;
            Completed = completed;
            Order = order;
        }

        public override bool Equals(object obj)
        {
            return obj is TodoTask task &&
                   Id.Equals(task.Id) &&
                   Title == task.Title &&
                   Completed == task.Completed &&
                   Order == task.Order;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Completed, Order);
        }
    }
}
