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

        public bool Equals(TodoTask other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id.Equals(other.Id) && Title == other.Title && Completed == other.Completed && Order == other.Order;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((TodoTask)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Completed, Order);
        }

        public static bool operator ==(TodoTask left, TodoTask right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TodoTask left, TodoTask right)
        {
            return !Equals(left, right);
        }
    }
}
