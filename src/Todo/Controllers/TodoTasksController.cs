using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Todo.API.Models;
using Todo.Domain.Models;
using Todo.Interfaces;

namespace Todo.Controllers
{
    [ApiController]
    [Route("TodoTasks")]
    public class TodoTasksController : ControllerBase
    {
        private readonly ILogger<TodoTasksController> _logger;
        private readonly ITodoTaskRepository _TodoRepository;

        public TodoTasksController(ILogger<TodoTasksController> logger, ITodoTaskRepository TodoRepository)
        {
            _logger = logger;
            _TodoRepository = TodoRepository;
        }

        [HttpGet]
        public IActionResult GetTodoTasks()
        {
            var todoTasks = _TodoRepository.GetTodoTasks();
            var uri = getBaseUri();

            return Ok(todoTasks.Select(todoTask => TodoTask.FromDAL(todoTask, uri)));
        }

        [HttpGet("{id}", Name = "GetTodoTask")]
        public IActionResult GetTodoTask(Guid id)
        {
            var todoTask = _TodoRepository.GetTodoTask(id);
            if (todoTask == null)
            {
                return NotFound();
            }
            return Ok(TodoTask.FromDAL(todoTask, getBaseUri()));
        }

        [HttpPost]
        public IActionResult CreateTodoTask(TodoTaskData todoTaskToCreate)
        {
            _TodoRepository.Add(todoTaskToCreate);
            var todoTask = TodoTask.FromDAL(todoTaskToCreate, getBaseUri());
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
            return Ok(TodoTask.FromDAL(todoTaskToUpdate, getBaseUri()));
        }

        private string getBaseUri()
        {
            var request = HttpContext.Request;
            var scheme = request.Scheme;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();

            return $"{scheme}://{host}{pathBase}/todotasks/";
        }
    }
}
