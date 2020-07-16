using System;
using Todo.Domain.Models;

namespace Todo.API.Models
{
    public class TodoTask
    {
        public static TodoTask FromDAL(TodoTaskData todoTask)
        {
            return new TodoTask(todoTask);
        }
        public TodoTask(TodoTaskData todoTask)
        {
            Id = todoTask.Id;
            Title = todoTask.Title;
            Completed = todoTask.Completed;
            Order = todoTask.Order;
            Url = todoTask.Url;
        }
        public Guid Id { get; }
        public string Title { get;}
        public bool Completed { get;}
        public int Order { get;}
        public Uri Url { get;}
    }

}
