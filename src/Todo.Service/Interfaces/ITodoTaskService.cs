using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Models;

namespace Todo.Domain.Interfaces
{
    public interface ITodoTaskService
    {
        Task<IEnumerable<TodoTask>> GetTodoTasksAsync();
        Task<TodoTask> GetTodoTaskAsync(Guid id);
        Task<TodoTask> AddAsync(string title, int order);
        Task<bool> RemoveAsync(Guid id);
        Task<TodoTask> UpdateAsync(Guid id, string title, bool? completed, int? order);
        Task ClearAsync();
    }
}
