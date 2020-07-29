using System;
using Todo.API.Models;
using Todo.Domain.Models;
using Xunit;

namespace Todo.Models.Tests
{
    public class TodoTaskShould
    {
        private readonly string Url = "https://arandomurl/";
        [Fact]
        public void BeCorrectlyInitializedWithConstructor()
        {
            var todoTask = new TodoTask(new TodoTaskData() { Id = Guid.NewGuid(), Title = "Try me", Order = 0, Completed = true }, Url);

            Assert.Equal("Try me", todoTask.Title);
            Assert.Equal(0, todoTask.Order);
            Assert.True(todoTask.Completed);
            Assert.Equal(todoTask.Url, new Uri(Url + todoTask.Id));
        }
    }
}
