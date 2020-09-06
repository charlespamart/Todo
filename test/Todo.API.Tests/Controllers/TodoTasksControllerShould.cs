using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Todo.API.Controllers;
using Todo.API.Models;
using Todo.Domain.Interfaces;
using Todo.Domain.Models;
using Xunit;

namespace Todo.API.Tests.Controllers
{
    public class TodoTasksControllerShould
    {
        private const string DefaultBaseUri = "https://arandomurl";

        static private readonly IImmutableList<Guid> _Guids = new List<Guid>() {
            new Guid("00000000-0000-0000-0000-000000000001"), 
            new Guid("00000000-0000-0000-0000-000000000002"),
            new Guid("00000000-0000-0000-0000-000000000003"),
            new Guid("00000000-0000-0000-0000-000000000004")
        }.ToImmutableList();

        private readonly Mock<ITodoTaskService> _mockTodoTaskService;

        private readonly TodoTasksController _todoTaskController;

        private readonly List<TodoTask> todoTasks;

        public TodoTasksControllerShould()
        {
            todoTasks = new List<TodoTask>
            {
                new TodoTask(_Guids[0], "Le gras, c'est la vie", true, 0),
                new TodoTask(_Guids[1], "C'est pas faux", false, 1),
                new TodoTask(_Guids[2], "Moi j'ai appris à lire, ben je souhaite ça à personne", true, 2),
                new TodoTask(_Guids[3], "Les mômes maintenant, ils lisent, ils lisent, ils lisent et résultat...ils sont encore puceaux à 10 ans...", false, 3),
            };

            _mockTodoTaskService = new Mock<ITodoTaskService>();

            Mock<LinkGenerator> mockLinkGenerator = new Mock<LinkGenerator>();

            mockLinkGenerator.Setup(lg => lg.GetUriByAddress(
                It.IsAny<HttpContext>(),
                It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(),
                It.IsAny<RouteValueDictionary>(),
                It.IsAny<string>(),
                It.IsAny<HostString?>(),
                It.IsAny<PathString?>(),
                It.IsAny<FragmentString>(),
                It.IsAny<LinkOptions>())).Returns(
                    (HttpContext httpContext,
                    RouteValuesAddress address,
                    RouteValueDictionary values,
                    RouteValueDictionary ambientValues,
                    string scheme,
                    HostString? host,
                    PathString? pathString,
                    FragmentString fragment,
                    LinkOptions options) => DefaultBaseUri + "/" + Guid.Parse(values["id"]?.ToString() ?? "00000000-0000-0000-0000-000000000000"));

            _todoTaskController = new TodoTasksController(_mockTodoTaskService.Object, mockLinkGenerator.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
        }

        [Fact]
        public async Task BeAbleToGetAllTodoTasksAsync()
        {
            var expected = todoTasks.Select(x => TodoTaskView.FromDomain(x, new Uri($"{DefaultBaseUri}/{x.Id}"))).ToImmutableList();

            _mockTodoTaskService.Setup(x => x.GetAllAsync()).ReturnsAsync(todoTasks);

            var actual = await _todoTaskController.GetAllAsync();

            Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(expected, ((OkObjectResult)actual).Value);

            _mockTodoTaskService.Verify(x => x.GetAllAsync(), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BeAbleToGetATodoTaskByIdAsync()
        {
            var guid = _Guids.First();

            var expected = TodoTaskView.FromDomain(todoTasks.Single(x => x.Id == guid), new Uri($"{DefaultBaseUri}/{guid}"));

            _mockTodoTaskService.Setup(x => x.GetByIdAsync(guid)).ReturnsAsync(todoTasks.Single(x => x.Id == guid));

            var actual = await _todoTaskController.GetByIdAsync(guid);

            Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(expected, ((OkObjectResult)actual).Value);

            _mockTodoTaskService.Verify(x => x.GetByIdAsync(guid), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReturnNullOnNonExistingTodoTaskByIdAsync()
        {
            var guid = _Guids.First();

            _mockTodoTaskService.Setup(x => x.GetByIdAsync(guid)).ReturnsAsync((TodoTask)null);

            var actual = await _todoTaskController.GetByIdAsync(guid);

            Assert.IsType<NotFoundResult>(actual);

            _mockTodoTaskService.Verify(x => x.GetByIdAsync(guid), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BeAbleToCreateANewTodoTaskAsync()
        {
            var guid = _Guids.First();
            var title = "C'est pas faux";
            var order = 1;

            var expected = TodoTaskView.FromDomain(todoTasks.Single(x => x.Id == guid), new Uri($"{DefaultBaseUri}/{guid}"));

            _mockTodoTaskService.Setup(x => x.AddAsync(title, order)).ReturnsAsync(todoTasks.Single(x => x.Id == guid));

            var todoTaskCreate = new TodoTaskCreate { Title = title, Order = order };

            var actual = await _todoTaskController.CreateAsync(todoTaskCreate);

            Assert.IsType<CreatedAtActionResult>(actual);
            Assert.Equal(expected, ((CreatedAtActionResult)actual).Value);

            _mockTodoTaskService.Verify(x => x.AddAsync(title, order), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BeAbleToPutTodoTaskAsync()
        {
            var guid = _Guids.First();
            var title = "De quoi?!";
            var completed = true;
            var order = 5;

            var todoTaskUpdate = new TodoTaskUpdate
            {
                Title = title,
                Completed = completed,
                Order = order
            };

            var expected = TodoTaskView.FromDomain(todoTasks.Single(x => x.Id == guid), new Uri($"{DefaultBaseUri}/{guid}"));

            _mockTodoTaskService.Setup(x => x.UpdateAsync(guid, title, completed, order)).ReturnsAsync(todoTasks.Single(x => x.Id == guid));

            var actual = await _todoTaskController.PutAsync(guid, todoTaskUpdate);

            Assert.IsType<AcceptedAtActionResult>(actual);
            Assert.Equal(expected, ((AcceptedAtActionResult)actual).Value);

            _mockTodoTaskService.Verify(x => x.UpdateAsync(guid, title, completed, order), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReturnBadRequestIfTitleIsNullWhenCallingPutAsync()
        {
            var guid = _Guids.First();
            var completed = true;
            var order = 5;

            var todoTaskUpdate = new TodoTaskUpdate
            {
                Title = null,
                Completed = completed,
                Order = order
            };

            var actual = await _todoTaskController.PutAsync(guid, todoTaskUpdate);

            Assert.IsType<BadRequestResult>(actual);

            _mockTodoTaskService.Verify(x => x.UpdateAsync(guid, null, completed, order), Times.Never());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReturnNotFoundOnNonExistingTodoTaskWhenCallingPutAsync()
        {
            var guid = Guid.NewGuid();
            var title = "De quoi?!";
            var completed = true;
            var order = 5;

            var todoTaskUpdate = new TodoTaskUpdate
            {
                Title = title,
                Completed = completed,
                Order = order
            };

            var actual = await _todoTaskController.PutAsync(guid, todoTaskUpdate);

            Assert.IsType<NotFoundResult>(actual);

            _mockTodoTaskService.Verify(x => x.UpdateAsync(guid, title, completed, order), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BeAbleToPatchAsync()
        {
            var guid = _Guids.First();
            var title = "Pour savoir s'y a du vent, il faut mettre son doigt dans le cul du coq";
            var completed = true;
            var order = 2;

            var expected = TodoTaskView.FromDomain(new TodoTask(guid, title, completed, order), new Uri($"{DefaultBaseUri}/{guid}"));

            var todoTaskUpdate = new TodoTaskUpdate
            {
                Title = title,
                Completed = completed,
                Order = order
            };

            _mockTodoTaskService.Setup(x => x.UpdateAsync(guid, title, completed, order)).ReturnsAsync(new TodoTask(guid, title, completed, order));

            var actual = await _todoTaskController.PatchAsync(guid, todoTaskUpdate);

            Assert.IsType<AcceptedAtActionResult>(actual);

            var actualValues = (TodoTaskView)((AcceptedAtActionResult)actual).Value;

            Assert.Equal(guid, actualValues.Id);
            Assert.Equal(title, actualValues.Title);
            Assert.Equal(completed, actualValues.Completed);
            Assert.Equal(order, actualValues.Order);
            Assert.Equal(new Uri($"{DefaultBaseUri}/{guid}"), actualValues.Url);

            _mockTodoTaskService.Verify(x => x.UpdateAsync(guid, title, completed, order), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReturnNotFoundOnNonExistingTodoTaskWhileCallingPatchAsync()
        {
            var guid = Guid.NewGuid();
            var title = "Mais vous êtes pas un peu marteau vous ?";
            var completed = true;
            var order = 5;

            var todoTaskUpdate = new TodoTaskUpdate
            {
                Title = "Mais vous êtes pas un peu marteau vous ?",
                Completed = true,
                Order = 5
            };

            var actual = await _todoTaskController.PatchAsync(guid, todoTaskUpdate);

            Assert.IsType<NotFoundResult>(actual);

            _mockTodoTaskService.Verify(x => x.UpdateAsync(guid, title, completed, order), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BeAbleToDeleteAllTodoTasksAsync()
        {
            var actual = await _todoTaskController.ClearAsync();

            Assert.IsType<NoContentResult>(actual);

            _mockTodoTaskService.Verify(x => x.ClearAsync(), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BeAbleToRemoveByIdAsync()
        {
            var guid = _Guids.First();

            _mockTodoTaskService.Setup(x => x.RemoveAsync(guid)).ReturnsAsync(true);

            var actual = await _todoTaskController.RemoveByIdAsync(guid);

            Assert.IsType<NoContentResult>(actual);

            _mockTodoTaskService.Verify(x => x.RemoveAsync(guid), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task NotBeAbleToRemoveNonExistingTodoTaskByIdAsync()
        {
            var guid = _Guids.First();

            _mockTodoTaskService.Setup(x => x.RemoveAsync(guid)).ReturnsAsync(false);

            var actual = await _todoTaskController.RemoveByIdAsync(guid);

            Assert.IsType<ConflictResult>(actual);

            _mockTodoTaskService.Verify(x => x.RemoveAsync(guid), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }
    }
}
