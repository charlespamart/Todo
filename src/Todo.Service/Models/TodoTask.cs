using System;
using Todo.DAL.Models;

namespace Todo.Business.Models
{
    public class TodoTask : IEquatable<TodoTask>
    {
        public static TodoTask FromDAL(TodoTaskData todoTaskData)
        {
            return new TodoTask(todoTaskData);
        }

        public TodoTask(TodoTaskData todoTaskData)
        {
            Id = todoTaskData.Id;
            Title = todoTaskData.Title;
            Order = todoTaskData.Order;
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int Order { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(TodoTask todoTask)
        {
            return todoTask != null && Id == todoTask.Id && Title == todoTask.Title && Completed == todoTask.Completed && Order == todoTask.Order;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
