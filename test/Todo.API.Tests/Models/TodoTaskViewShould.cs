using System;
using Todo.API.Models;
using Todo.Domain.Models;
using Xunit;

namespace Todo.API.Tests.Models
{
    public class TodoTaskViewShould
    {
        private const string Url = "https://arandomurl";

        [Fact]
        public void MapATodoTaskViewToATodoTask()
        {
            var todoTask = new TodoTask(Guid.NewGuid(), "Todotask", true, 0);
            var url = new Uri($"{Url}/{todoTask.Id}");
            var todoTaskView = TodoTaskView.FromDomain(todoTask, url));

            Assert.Equal(todoTask.Id, todoTaskView.Id);
            Assert.Equal(todoTask.Title, todoTaskView.Title);
            Assert.Equal(todoTask.Order, todoTaskView.Order);
            Assert.Equal(todoTask.Completed, todoTaskView.Completed);
            Assert.Equal(url, todoTaskView.Url);
        }
    }
}
