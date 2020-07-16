using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly ITodoRepository _TodoRepository;

        public TodoTasksController(ILogger<TodoTasksController> logger, ITodoRepository TodoRepository)
        {
            _logger = logger;
            _TodoRepository = TodoRepository;
        }

        [HttpGet]
        public IActionResult GetTodoTasks()
        {
            return Ok(_TodoRepository.GetTodoTasks());
        }

        [HttpGet("{id}", Name = "GetTodoTask")]
        public IActionResult GetTodoTask(Guid id)
        {
            var todoTask = _TodoRepository.GetTodoTask(id);
            if (todoTask == null)
            {
                return NotFound();
            }
            return Ok(TodoTask.FromDAL(todoTask));
        }

        [HttpPost]
        public IActionResult CreateTodoTask(TodoTaskData todoTaskToCreate)
        {
            _TodoRepository.Add(todoTaskToCreate);
            var todoTask = TodoTask.FromDAL(todoTaskToCreate);
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
            return Ok(todoTaskToUpdate);
        }
    }
}
