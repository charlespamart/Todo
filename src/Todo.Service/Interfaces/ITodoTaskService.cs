using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Todo.Domain.Models;

namespace Todo.Domain.Interfaces
{
    public interface ITodoTaskService
    {
        Task<IEnumerable<TodoTask>> GetAllAsync();
        Task<TodoTask> GetByIdAsync(Guid id);
        Task<TodoTask> AddAsync(string title, int order);
        Task<bool> RemoveAsync(Guid id);
        Task<TodoTask> UpdateAsync(Guid id, string title, bool? completed, int? order);
        Task<IImmutableList<TodoTask>> UpdateAllCompletedStateAsync(bool completed);
        Task<bool> ClearAsync();
        Task<ImmutableList<TodoTask>> ClearAllCompletedAsync();
    }
}
