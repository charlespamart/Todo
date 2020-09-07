using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Todo.Domain.Models;

namespace Todo.DAL.Models
{
    public class TodoTaskData : IEquatable<TodoTaskData>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int Order { get; set; }

        public TodoTask ToDomain()
        {
            return new TodoTask(Id, Title, Completed, Order);
        }

        public bool Equals(TodoTaskData other)
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
            return Equals((TodoTaskData)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Completed, Order);
        }

        public static bool operator ==(TodoTaskData left, TodoTaskData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TodoTaskData left, TodoTaskData right)
        {
            return !Equals(left, right);
        }
    }
}
