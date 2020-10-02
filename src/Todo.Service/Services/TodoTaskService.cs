using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Todo.Domain.Interfaces;
using Todo.Domain.Models;

namespace Todo.Domain
{
    public class TodoTaskService : ITodoTaskService
    {
        private readonly ITodoTaskRepository _todoRepository;

        public TodoTaskService(ITodoTaskRepository todoTaskRepository)
        {
            _todoRepository = todoTaskRepository;
        }

        public async Task<IEnumerable<TodoTask>> GetAllAsync()
        {
            return await _todoRepository.GetAllAsync();
        }
        public async Task<TodoTask> GetByIdAsync(Guid id)
        {
            return await _todoRepository.GetByIdAsync(id);
        }
        public async Task<TodoTask> AddAsync(string title, int order)
        {
            return await _todoRepository.AddAsync(title, order);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            return await _todoRepository.RemoveAsync(id);
        }
        public async Task<TodoTask> UpdateAsync(Guid id, string title, bool? completed, int? order)
        {
            return await _todoRepository.UpdateAsync(id, title, completed, order);
        }

        public async Task<ImmutableList<TodoTask>> UpdateAllCompletedStateAsync(bool completed)
        {
            return await _todoRepository.UpdateAllCompletedStateAsync(completed);
        }

        public async Task<bool> ClearAsync()
        {
            return await _todoRepository.ClearAsync();
        }
    }
}
