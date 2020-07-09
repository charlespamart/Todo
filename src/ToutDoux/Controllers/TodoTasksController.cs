using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todo.Interfaces;
using Todo.Models;

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
        public TodoTask[] GetTodoTasks()
        {
            return _TodoRepository.GetTodoTasks();
        }

        [HttpGet("{id}", Name = "GetTodoTask")]
        public TodoTask GetTodoTask(long id)
        {
            return _TodoRepository.GetTodoTask(id);
        }

        [HttpPost]
        public IActionResult CreateTodoTask(TodoTask TodoTask)
        {
            _TodoRepository.Add(TodoTask);
            return CreatedAtRoute("GetTodoTask", new { id = TodoTask.Id }, TodoTask);
        }

        [HttpDelete("{id}")]
        public void RemoveTodoTask(long id)
        {
            _TodoRepository.Remove(id);
        }

        [HttpDelete]
        public void RemoveTodoTasks()
        {
            _TodoRepository.Clear();
        }

        [HttpPatch("{id}")]
        public TodoTask UpdateTodoTask(long id, TodoTask TodoTask)
        {
            return _TodoRepository.Update(id, TodoTask);
        }
    }
}
