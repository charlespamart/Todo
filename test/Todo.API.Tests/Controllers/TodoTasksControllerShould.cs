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

        private static readonly Guid _guid = Guid.Empty;

        private readonly Mock<ITodoTaskService> _mockTodoTaskService;

        private readonly TodoTasksController _todoTaskController;

        private readonly IImmutableList<TodoTask> _todoTasks = new List<TodoTask>
            {
                new TodoTask(new Guid("00000000-0000-0000-0000-000000000000"), "Todotask 0", true, 0),
                new TodoTask(new Guid("00000000-0000-0000-0000-000000000001"), "Todotask 1", false, 1),
                new TodoTask(new Guid("00000000-0000-0000-0000-000000000002"), "Todotask 2", true, 2),
                new TodoTask(new Guid("00000000-0000-0000-0000-000000000003"), "Todotask 3", false, 3),
            }.ToImmutableList();

        public TodoTasksControllerShould()
        {
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
            var expected = _todoTasks.Select(x => TodoTaskView.FromDomain(x, new Uri($"{DefaultBaseUri}/{x.Id}"))).ToImmutableList();

            _mockTodoTaskService.Setup(x => x.GetAllAsync()).ReturnsAsync(_todoTasks);

            var actual = await _todoTaskController.GetAllAsync();

            Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(expected, ((OkObjectResult)actual).Value);

            _mockTodoTaskService.Verify(x => x.GetAllAsync(), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BeAbleToGetATodoTaskByIdAsync()
        {
            var expected = TodoTaskView.FromDomain(_todoTasks.Single(x => x.Id == _guid), new Uri($"{DefaultBaseUri}/{_guid}"));

            _mockTodoTaskService.Setup(x => x.GetByIdAsync(_guid)).ReturnsAsync(_todoTasks.Single(x => x.Id == _guid));

            var actual = await _todoTaskController.GetByIdAsync(_guid);

            Assert.IsType<OkObjectResult>(actual);
            Assert.Equal(expected, ((OkObjectResult)actual).Value);

            _mockTodoTaskService.Verify(x => x.GetByIdAsync(_guid), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReturnNullOnNonExistingTodoTaskByIdAsync()
        {
            _mockTodoTaskService.Setup(x => x.GetByIdAsync(_guid)).ReturnsAsync((TodoTask)null);

            var actual = await _todoTaskController.GetByIdAsync(_guid);

            Assert.IsType<NotFoundResult>(actual);

            _mockTodoTaskService.Verify(x => x.GetByIdAsync(_guid), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BeAbleToCreateANewTodoTaskAsync()
        {
            const string title = "Todotask 0";
            const int order = 0;

            var expected = TodoTaskView.FromDomain(_todoTasks.Single(x => x.Id == _guid), new Uri($"{DefaultBaseUri}/{_guid}"));

            _mockTodoTaskService.Setup(x => x.AddAsync(title, order)).ReturnsAsync(_todoTasks.Single(x => x.Id == _guid));

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
            const string title = "Todotask";
            const bool completed = true;
            const int order = 5;

            var todoTaskUpdate = new TodoTaskUpdate
            {
                Title = title,
                Completed = completed,
                Order = order
            };

            var expected = TodoTaskView.FromDomain(_todoTasks.Single(x => x.Id == _guid), new Uri($"{DefaultBaseUri}/{_guid}"));

            _mockTodoTaskService.Setup(x => x.UpdateAsync(_guid, title, completed, order)).ReturnsAsync(_todoTasks.Single(x => x.Id == _guid));

            var actual = await _todoTaskController.PutAsync(_guid, todoTaskUpdate);

            Assert.IsType<AcceptedAtActionResult>(actual);
            Assert.Equal(expected, ((AcceptedAtActionResult)actual).Value);

            _mockTodoTaskService.Verify(x => x.UpdateAsync(_guid, title, completed, order), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReturnBadRequestIfTitleIsNullWhenCallingPutAsync()
        {
            const bool completed = true;
            const int order = 5;

            var todoTaskUpdate = new TodoTaskUpdate
            {
                Title = null,
                Completed = completed,
                Order = order
            };

            var actual = await _todoTaskController.PutAsync(_guid, todoTaskUpdate);

            Assert.IsType<BadRequestResult>(actual);

            _mockTodoTaskService.Verify(x => x.UpdateAsync(_guid, null, completed, order), Times.Never());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReturnNotFoundOnNonExistingTodoTaskWhenCallingPutAsync()
        {
            var guid = Guid.NewGuid();
            const string title = "Todotask";
            const bool completed = true;
            const int order = 5;

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
            const string title = "Todotask";
            const bool completed = true;
            const int order = 2;

            var expected = TodoTaskView.FromDomain(new TodoTask(_guid, title, completed, order), new Uri($"{DefaultBaseUri}/{_guid}"));

            var todoTaskUpdate = new TodoTaskUpdate
            {
                Title = title,
                Completed = completed,
                Order = order
            };

            _mockTodoTaskService.Setup(x => x.UpdateAsync(_guid, title, completed, order)).ReturnsAsync(new TodoTask(_guid, title, completed, order));

            var actual = await _todoTaskController.PatchAsync(_guid, todoTaskUpdate);

            Assert.IsType<AcceptedAtActionResult>(actual);

            var actualValues = (TodoTaskView)((AcceptedAtActionResult)actual).Value;

            Assert.Equal(_guid, actualValues.Id);
            Assert.Equal(title, actualValues.Title);
            Assert.Equal(completed, actualValues.Completed);
            Assert.Equal(order, actualValues.Order);
            Assert.Equal(new Uri($"{DefaultBaseUri}/{_guid}"), actualValues.Url);

            _mockTodoTaskService.Verify(x => x.UpdateAsync(_guid, title, completed, order), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReturnNotFoundOnNonExistingTodoTaskWhileCallingPatchAsync()
        {
            var _guid = Guid.NewGuid();
            const string title = "Todotask";
            const bool completed = true;
            const int order = 5;

            var todoTaskUpdate = new TodoTaskUpdate
            {
                Title = title,
                Completed = completed,
                Order = order
            };

            var actual = await _todoTaskController.PatchAsync(_guid, todoTaskUpdate);

            Assert.IsType<NotFoundResult>(actual);

            _mockTodoTaskService.Verify(x => x.UpdateAsync(_guid, title, completed, order), Times.Once());
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

            _mockTodoTaskService.Setup(x => x.RemoveAsync(_guid)).ReturnsAsync(true);

            var actual = await _todoTaskController.RemoveByIdAsync(_guid);

            Assert.IsType<NoContentResult>(actual);

            _mockTodoTaskService.Verify(x => x.RemoveAsync(_guid), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task NotBeAbleToRemoveNonExistingTodoTaskByIdAsync()
        {

            _mockTodoTaskService.Setup(x => x.RemoveAsync(_guid)).ReturnsAsync(false);

            var actual = await _todoTaskController.RemoveByIdAsync(_guid);

            Assert.IsType<ConflictResult>(actual);

            _mockTodoTaskService.Verify(x => x.RemoveAsync(_guid), Times.Once());
            _mockTodoTaskService.VerifyNoOtherCalls();
        }
    }
}
