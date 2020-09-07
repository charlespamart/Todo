using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;
using Todo.API.Models;
using Todo.Domain.Interfaces;
using Todo.Domain.Models;

namespace Todo.API.Controllers
{
    [ApiController]
    [Route(ControllerName)]
    public class TodoTasksController : ControllerBase
    {
        private readonly ITodoTaskService _todoTaskService;
        private readonly LinkGenerator _linkGenerator;
        private const string ControllerName = "todotasks";

        public TodoTasksController(ITodoTaskService todoTaskService, LinkGenerator linkGenerator)
        {
            _todoTaskService = todoTaskService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ActionName(nameof(GetAllAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var todoTasks = await _todoTaskService.GetAllAsync();
            return Ok(todoTasks.Select(todoTask => TodoTaskView.FromDomain(todoTask, GetResourceUri(todoTask.Id))));
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetByIdAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var todoTask = await _todoTaskService.GetByIdAsync(id);
            if (todoTask == null)
            {
                return NotFound();
            }
            return Ok(TodoTaskView.FromDomain(todoTask, GetResourceUri(todoTask.Id)));
        }

        [HttpPost]
        [ActionName(nameof(CreateAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(TodoTaskCreate todoTaskCreate)
        {
            var todoTaskAdd = await _todoTaskService.AddAsync(todoTaskCreate.Title, todoTaskCreate.Order);

            if(todoTaskAdd.Title == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetByIdAsync), new { id = todoTaskAdd.Id }, TodoTaskView.FromDomain(todoTaskAdd, GetResourceUri(todoTaskAdd.Id)));
        }

        [HttpDelete("{id:guid}")]
        [ActionName(nameof(RemoveByIdAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RemoveByIdAsync(Guid id)
        {
            if ( await _todoTaskService.RemoveAsync(id))
            {
                return NoContent();
            }
            return Conflict();
        }

        [HttpDelete]
        [ActionName(nameof(ClearAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ClearAsync()
        {
            await _todoTaskService.ClearAsync();
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ActionName(nameof(PutAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(Guid id, TodoTaskUpdate todoTaskPut)
        {
            if (todoTaskPut.Title == null || !todoTaskPut.Completed.HasValue || !todoTaskPut.Order.HasValue)
            {
                return BadRequest();
            }

            var todoTaskUpdated = await _todoTaskService.UpdateAsync(id, todoTaskPut.Title, todoTaskPut.Completed.Value, todoTaskPut.Order.Value);

            if (todoTaskUpdated == null)
            {
                return NotFound();
            }
            return AcceptedAtAction(nameof(PutAsync), new { id = todoTaskUpdated.Id }, TodoTaskView.FromDomain(todoTaskUpdated, GetResourceUri(todoTaskUpdated.Id)));
        }

        [HttpPatch("{id:guid}")]
        [ActionName(nameof(PatchAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchAsync(Guid id, TodoTaskUpdate todoTaskPatch)
        {
            var todoTaskUpdated = await _todoTaskService.UpdateAsync(id, todoTaskPatch.Title, todoTaskPatch.Completed, todoTaskPatch.Order);

            if (todoTaskUpdated == null)
            {
                return NotFound();
            }
            return AcceptedAtAction(nameof(PatchAsync), new { id = todoTaskUpdated.Id }, TodoTaskView.FromDomain(todoTaskUpdated, GetResourceUri(todoTaskUpdated.Id)));
        }

        private Uri GetResourceUri(Guid id)
        {
            return new Uri(_linkGenerator.GetUriByAction(HttpContext, nameof(GetByIdAsync), ControllerName, new { id }));
        }
    }
}
