using System;
using Todo.Business.Models;

namespace Todo.API.Models
{
    public class TodoTaskView: IEquatable<TodoTaskView>
    {
        public static TodoTaskView FromBusiness(TodoTask todoTaskView, string path)
        {
            return new TodoTaskView(todoTaskView, path);
        }

        public TodoTaskView(TodoTask todoTaskView, string path)
        {
            Id = todoTaskView.Id;
            Title = todoTaskView.Title;
            Completed = todoTaskView.Completed;
            Order = todoTaskView.Order;
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

        public bool Equals(TodoTaskView todoTaskView)
        {
            return todoTaskView != null && Id == todoTaskView.Id && Title == todoTaskView.Title && Completed == todoTaskView.Completed && Order == todoTaskView.Order;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
