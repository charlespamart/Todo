using System;
using Todo.Domain.Models;

namespace Todo.API.Models
{
    public class TodoTaskView
    {
        public Guid Id { get; }
        public string Title { get; }
        public bool Completed { get; }
        public int Order { get; }
        public Uri Url { get; }

        public static TodoTaskView FromDomain(TodoTask todoTask, Uri path)
        {
            return new TodoTaskView(todoTask, path);
        }

        public TodoTaskView(TodoTask todoTask, Uri path)
        {
            Id = todoTask.Id;
            Title = todoTask.Title;
            Completed = todoTask.Completed;
            Order = todoTask.Order;
            Url = path;
        }
    }

}
