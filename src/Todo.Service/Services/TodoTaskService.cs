using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Domain.Interfaces;
using Todo.Domain.Models;

namespace Todo.Domain
{
    public class TodoTaskService : ITodoTaskService
    {
        private readonly ITodoTaskRepository _TodoRepository;

        public TodoTaskService(ITodoTaskRepository todoTaskRepository)
        {
            _TodoRepository = todoTaskRepository;
        }

        public async Task<IEnumerable<TodoTask>> GetTodoTasksAsync()
        {
            return await _TodoRepository.GetTodoTasksAsync();
        }
        public async Task<TodoTask> GetTodoTaskAsync(Guid id)
        {
            return await _TodoRepository.GetTodoTaskAsync(id);
        }
        public async Task<TodoTask> AddAsync(string title, int order)
        {
            return await _TodoRepository.AddAsync(title, order);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            return await _TodoRepository.RemoveAsync(id);
        }
        public async Task<TodoTask> UpdateAsync(Guid id, string title, bool? completed, int? order)
        {
            return await _TodoRepository.UpdateAsync(id, title, completed, order);
        }
        public async Task ClearAsync()
        {
            await _TodoRepository.ClearAsync();
        }
    }
}
