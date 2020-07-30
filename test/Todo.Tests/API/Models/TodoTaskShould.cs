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
        public void BeInitializedWithConstructor()
        {
            var id = Guid.NewGuid();
            var todoTask = new TodoTaskView(new TodoTaskData() { Id = id, Title = "Try me", Order = 0, Completed = true }, Url);

            Assert.Equal(id, todoTask.Id);
            Assert.Equal("Try me", todoTask.Title);
            Assert.Equal(0, todoTask.Order);
            Assert.True(todoTask.Completed);
            Assert.Equal(todoTask.Url, new Uri(Url + todoTask.Id));
        }

        [Fact]
        public void ConvertATodoTaskDataToATodoTask()
        {
            var id = Guid.NewGuid();
            var todoTaskData = new TodoTaskData() { Id = id, Title = "Try me", Order = 0, Completed = true };
            var todoTask = TodoTaskView.FromDAL(todoTaskData, Url);

            Assert.Equal(id, todoTask.Id);
            Assert.Equal("Try me", todoTask.Title);
            Assert.Equal(0, todoTask.Order);
            Assert.True(todoTask.Completed);
            Assert.Equal(todoTask.Url, new Uri(Url + todoTask.Id));
        }
    }
}
