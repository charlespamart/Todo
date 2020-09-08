using System;
using Todo.Domain.Models;

namespace Todo.API.Models
{
    public class TodoTaskView : IEquatable<TodoTaskView>
    {
        public Guid Id { get; }
        public string Title { get; }
        public bool Completed { get; }
        public int Order { get; }
        public Uri Url { get; }

        private TodoTaskView(TodoTask todoTask, Uri path)
        {
            Id = todoTask.Id;
            Title = todoTask.Title;
            Completed = todoTask.Completed;
            Order = todoTask.Order;
            Url = path;
        }

        public static TodoTaskView FromDomain(TodoTask todoTask, Uri path)
        {
            return new TodoTaskView(todoTask, path);
        }

        public bool Equals(TodoTaskView other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id.Equals(other.Id) && Title == other.Title && Completed == other.Completed && Order == other.Order && Equals(Url, other.Url);
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
            return Equals((TodoTaskView)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Completed, Order, Url);
        }

        public static bool operator ==(TodoTaskView left, TodoTaskView right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TodoTaskView left, TodoTaskView right)
        {
            return !Equals(left, right);
        }

    }

}
