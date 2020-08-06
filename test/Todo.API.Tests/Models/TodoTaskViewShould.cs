using System;
using Todo.API.Models;
using Todo.Domain.Models;
using Xunit;

namespace Todo.API.Tests.Models
{
    public class TodoTaskViewShould
    {
        private readonly string Url = "https://arandomurl";

        [Fact]
        public void BeInitializedWithConstructor()
        {
            var id = Guid.NewGuid();
            var todoTaskView = new TodoTaskView(new TodoTask (id, "Oui mon seigneur ?", true, 0 ), new Uri($"{Url}/{id}"));

            Assert.Equal(id, todoTaskView.Id);
            Assert.Equal("Oui mon seigneur ?", todoTaskView.Title);
            Assert.Equal(0, todoTaskView.Order);
            Assert.True(todoTaskView.Completed);
            Assert.Equal(todoTaskView.Url, new Uri($"{Url}/{todoTaskView.Id}"));
        }

        [Fact]
        public void MapATodoTaskViewToATodoTask()
        {
            var todoTask = new TodoTask(Guid.NewGuid(), "C'est le bon choix. Bien d'accord !", true, 0);
            var todoTaskView = TodoTaskView.FromDomain(todoTask, new Uri($"{Url}/{todoTask.Id}"));

            Assert.Equal(todoTask.Id, todoTaskView.Id);
            Assert.Equal(todoTask.Title, todoTaskView.Title);
            Assert.Equal(todoTask.Order, todoTaskView.Order);
            Assert.Equal(todoTask.Completed, todoTaskView.Completed);
        }
    }
}
