using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Todo.Controllers
{
    [ApiController]
    [Route("TodoTasks")]
    public class TodoTasksController : ControllerBase
    {
        private readonly ILogger<TodoTasksController> _logger;
        private readonly ITodoTaskRepository _TodoRepository;

        public TodoTasksController(ITodoTaskRepository TodoRepository)
        {
            _TodoRepository = TodoRepository;
        }

        [HttpGet]
        public IActionResult GetTodoTasks()
        {
            var todoTasks = _TodoRepository.GetTodoTasks();
            var uri = GetBaseUri();


            return Ok(todoTasks.Select(todoTask => TodoTaskView.FromDAL(todoTask, uri)));
        }

        [HttpGet("{id}", Name = "GetTodoTask")]
        public IActionResult GetTodoTask(Guid id)
        {
            var todoTask = _TodoRepository.GetTodoTask(id);
            if (todoTask == null)
            {
                return NotFound();
            }
            return Ok(TodoTaskView.FromDAL(todoTask, GetBaseUri()));
        }

        [HttpPost]
        public IActionResult CreateTodoTask(TodoTaskData todoTaskToCreate)
        {
            _TodoRepository.Add(todoTaskToCreate);
            var todoTask = TodoTaskView.FromDAL(todoTaskToCreate, GetBaseUri());
            return CreatedAtRoute("GetTodoTask", new { id = todoTask.Id }, todoTask);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveTodoTask(Guid id)
        {
            var todoTask = _TodoRepository.GetTodoTask(id);
            if (todoTask == null)
            {
                return NotFound();
            }
            _TodoRepository.Remove(todoTask);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult RemoveTodoTasks()
        {
            _TodoRepository.Clear();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateTodoTask(Guid id, TodoTaskData todoTask)
        {
            var todoTaskToUpdate = _TodoRepository.GetTodoTask(id);
            if (todoTaskToUpdate == null)
            {
                return NotFound();
            }
            todoTaskToUpdate.Title = todoTask.Title;
            todoTaskToUpdate.Order = todoTask.Order;
            todoTaskToUpdate.Completed = todoTask.Completed;
            _TodoRepository.Update(todoTaskToUpdate);
            return Ok(TodoTaskView.FromDAL(todoTaskToUpdate, GetBaseUri()));
        }

        private string GetBaseUri()
        {
            var request = HttpContext.Request;
            var scheme = request.Scheme;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();

            return $"{scheme}://{host}{pathBase}/todotasks/";
        }
    }
}
