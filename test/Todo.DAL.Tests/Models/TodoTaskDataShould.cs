using System;
using Todo.DAL.Models;
using Xunit;

namespace Todo.DAL.Tests.Models
{
    public class TodoTaskDataShould
    {
        [Fact]
        public void BeInitializedWithConstructor()
        {
            var id = Guid.NewGuid();
            var todoTaskData = new TodoTaskData { Id = id, Title = "Vous voulez un whisky ?", Order = 0, Completed = true };

            Assert.Equal(id, todoTaskData.Id);
            Assert.Equal("Vous voulez un whisky ?", todoTaskData.Title);
            Assert.Equal(0, todoTaskData.Order);
            Assert.True(todoTaskData.Completed);
        }

        [Fact]
        public void InitializeCompletedToFalseIfNoValueIsGiven()
        {
            var todoTaskData = new TodoTaskData { Id = Guid.NewGuid(), Title = "- Oh juste un doigt", Order = 0 };

            Assert.False(todoTaskData.Completed);
        }

        [Fact]
        public void InitializeOrderToZeroIfNoValueIsGiven()
        {
            var todoTaskData = new TodoTaskData { Id = Guid.NewGuid(), Title = "Vous ne voulez pas un whisky d'abord ?", Completed = true };

            Assert.Equal(0, todoTaskData.Order);
        }
    }
}
