using Microsoft.AspNetCore.Http;
using System;

namespace Todo.Domain.Models
{
    public class TodoTaskData : IEquatable<TodoTaskData>
    {
        public Guid Id{ get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int Order { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(TodoTaskData todoTaskData)
        {
            return todoTaskData != null && Id == todoTaskData.Id && Title == todoTaskData.Title && Completed == todoTaskData.Completed && Order == todoTaskData.Order;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
