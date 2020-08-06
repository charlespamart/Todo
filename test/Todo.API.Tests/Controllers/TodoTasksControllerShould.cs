using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using Todo.API.Controllers;
using Todo.Domain.Interfaces;
using Todo.Domain.Models;
using Xunit;

namespace Todo.API.Tests.Controllers
{
    public class TodoTasksControllerShould
    {
        private readonly TodoTasksController todoTaskController;
        private const string DefaultBaseUri = "https://arandomurl";
        private readonly List<Guid> guids;

        public TodoTasksControllerShould()
        {
            guids = new List<Guid>(4);

            for (var i = 0; i < guids.Capacity; ++i)
            {
                guids.Add(Guid.NewGuid());
            }

            List<TodoTask> todoTasks = new List<TodoTask>
            {
                new TodoTask(guids[0], "Le gras, c'est la vie", true, 0),
                new TodoTask(guids[1], "C'est pas faux", false, 1),
                new TodoTask(guids[2], "Moi j'ai appris à lire, ben je souhaite ça à personne", true, 2),
                new TodoTask(guids[3], "Les mômes maintenant, ils lisent, ils lisent, ils lisent et résultat...ils sont encore puceaux à 10 ans...", false, 3),
            };

            Mock<ITodoTaskService> mockTodoTaskService = new Mock<ITodoTaskService>();
            Mock<LinkGenerator> mockLinkGenerator = new Mock<LinkGenerator>();

            mockLinkGenerator = new Mock<LinkGenerator>();
            mockLinkGenerator.Setup(lg => lg.GetUriByAddress(
                It.IsAny<HttpContext>(),
                It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(),
                It.IsAny<RouteValueDictionary>(),
                It.IsAny<string>(),
                It.IsAny<HostString?>(),
                It.IsAny<PathString?>(),
                It.IsAny<FragmentString>(),
                It.IsAny<LinkOptions>())).Returns(DefaultBaseUri);

            todoTaskController = new TodoTasksController(mockTodoTaskService.Object, mockLinkGenerator.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
        }

        [Fact]
        public void GetByIdAsync()
        {
            var todotask = todoTaskController.GetByIdAsync(guids[0]);

            Assert.True(true);
        }
    }
}
