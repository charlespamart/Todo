using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Todo.Interfaces;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    [Route("Todo")]
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
        public TodoTask[] Get()
        {
            return _TodoRepository.GetTodoTasks();
        }

        [HttpGet("{id}", Name = "GetById")]
        public TodoTask GetById(int id)
        {
            return _TodoRepository.GetTodoTask(id);
        }

        [HttpPost]
        public IActionResult Post(TodoTask TodoTask)
        {
            _TodoRepository.Add(TodoTask);
            return CreatedAtRoute("GetById", new { id = TodoTask.Id }, TodoTask);
        }

        [HttpDelete]
        public void Delete()
        {
            _TodoRepository.RemoveAll();
        }
    }
}
