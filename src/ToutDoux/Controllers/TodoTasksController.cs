using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todo.Interfaces;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    [Route("Todos")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly ITodoRepository _TodoRepository;

        public TodoController(ILogger<TodoController> logger, ITodoRepository TodoRepository)
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
        public TodoTask GetTodoTask(int id)
        {
            return _TodoRepository.GetTodoTask(id);
        }

        [HttpPost]
        public IActionResult CreateTodoTask(TodoTask TodoTask)
        {
            _TodoRepository.Add(TodoTask);
            return CreatedAtRoute("GetTodoTask", new { id = TodoTask.Id }, TodoTask);
        }

        [HttpDelete]
        public void DeleteTodoTasks()
        {
            _TodoRepository.Clear();
        }
    }
}
