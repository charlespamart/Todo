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

        public async Task<IEnumerable<TodoTask>> GetAllAsync()
        {
            return await _TodoRepository.GetAllAsync();
        }
        public async Task<TodoTask> GetByIdAsync(Guid id)
        {
            return await _TodoRepository.GetByIdAsync(id);
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
