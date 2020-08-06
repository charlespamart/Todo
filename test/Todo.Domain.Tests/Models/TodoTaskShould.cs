using System;
using Todo.DAL.Models;
using Xunit;

namespace Todo.Domain.Models.Tests
{
    public class TodoTaskShould
    {
        [Fact]
        public void BeInitializedWithConstructor()
        {
            var id = Guid.NewGuid();
            var todoTask = new TodoTask(id, "Pas de pierre : pas de construction", true, 0);

            Assert.Equal(id, todoTask.Id);
            Assert.Equal("Pas de pierre : pas de construction", todoTask.Title);
            Assert.Equal(0, todoTask.Order);
            Assert.True(todoTask.Completed);
        }

        [Fact]
        public void MapATodoTaskDataToATodoTask()
        {
            var todoTaskData = new TodoTaskData { Id = Guid.NewGuid(), Title = "Il est bizarre ce sol, il est pas palpable", Order = 0, Completed = true };
            var todoTask = todoTaskData.ToDomain();

            Assert.Equal(todoTaskData.Id, todoTask.Id);
            Assert.Equal(todoTaskData.Title, todoTask.Title);
            Assert.Equal(todoTaskData.Order, todoTask.Order);
            Assert.Equal(todoTaskData.Completed, todoTask.Completed);
        }
    }
}
