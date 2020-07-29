using System;
using Todo.API.Models;
using Todo.Domain.Models;
using Xunit;

namespace Todo.Tests.Models
{
    class TodoTaskDataShould
    {
        public void BeCorrectlyInitializedWithConstructor()
        {
            var todoTaskData = new TodoTaskData() {Id = Guid.NewGuid(), Title = "Hello there", Order = 0, Completed = false };

            Assert.Equal("Try me", todoTaskData.Title);
            Assert.Equal(0, todoTaskData.Order);
            Assert.True(todoTaskData.Completed);
        }
    }
}
