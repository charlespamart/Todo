using System;
using System.Collections.Generic;
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

        public override bool Equals(object obj)
        {
            return obj is TodoTaskView view &&
                   Id.Equals(view.Id) &&
                   Title == view.Title &&
                   Completed == view.Completed &&
                   Order == view.Order &&
                   EqualityComparer<Uri>.Default.Equals(Url, view.Url);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Completed, Order, Url);
        }
    }

}
