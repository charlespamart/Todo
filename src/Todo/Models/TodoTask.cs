using System;
using Todo.Domain.Models;

namespace Todo.API.Models
{
    public class TodoTask: IEquatable<TodoTask>
    {
        public static TodoTask FromDAL(TodoTaskData todoTaskData, string path)
        {
            return new TodoTask(todoTaskData, path);
        }

        public TodoTask(TodoTaskData todoTask, string path)
        {
            Id = todoTask.Id;
            Title = todoTask.Title;
            Completed = todoTask.Completed;
            Order = todoTask.Order;
            Url = new Uri(path + Id);
        }
        public Guid Id { get; }
        public string Title { get;}
        public bool Completed { get;}
        public int Order { get;}
        public Uri Url { get;}

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
