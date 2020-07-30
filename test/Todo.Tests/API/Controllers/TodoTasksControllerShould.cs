using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Controllers;
using Todo.Domain.Models;
using Todo.Interfaces;
using Xunit;

namespace Todo.Tests.Controllers
{
    public class TodoTasksControllerShould
    {
        private readonly TodoTasksController todoTaskController;
        private readonly Guid testingId;
        public TodoTasksControllerShould()
        {
            testingId = Guid.NewGuid();
            List<TodoTaskData> todoTaskDatas = new List<TodoTaskData>
            {
                new TodoTaskData { Id = Guid.NewGuid(), Title = "Never gonna give you up",  Completed = false, Order = 0},
                new TodoTaskData { Id = Guid.NewGuid(), Title = "Never gonna let you down",  Completed = true, Order = 1},
                new TodoTaskData { Id = Guid.NewGuid(), Title = "Never gonna run around and desert you",  Completed = false, Order = 2},
                new TodoTaskData { Id = testingId, Title = "Never gonna make you cry",  Completed = true, Order = 3},
            };
            Mock<ITodoTaskRepository> mockTodoTaskRepository = new Mock<ITodoTaskRepository>();

            mockTodoTaskRepository.Setup(tdtr => tdtr.GetTodoTasks()).Returns(todoTaskDatas);

            mockTodoTaskRepository.Setup(tdtr => tdtr.GetTodoTask(It.IsAny<Guid>())).Returns((Guid id) => todoTaskDatas.Where(x => x.Id == id).FirstOrDefault());

            todoTaskController = new TodoTasksController(mockTodoTaskRepository.Object);
        }

        [Fact]
        public void GetATodoTaskById()
        {
            var expected = new TodoTaskData { Id = testingId, Title = "Never gonna make you cry", Completed = true, Order = 3 };

            var result = todoTaskController.GetTodoTask(testingId) as OkObjectResult;

            Assert.NotNull(expected);
            Assert.Equal(expected, result.Value);
        }
    }
}
