﻿using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Todo.Domain.Models;

namespace Todo.Domain.Interfaces
{
    public interface ITodoTaskRepository
    {
        Task<IImmutableList<TodoTask>> GetAllAsync();
        Task<TodoTask> GetByIdAsync(Guid id);
        Task<TodoTask> AddAsync(string title, int order);
        Task<bool> RemoveAsync(Guid id);
        Task<TodoTask> UpdateAsync(Guid id, string title, bool? completed, int? order);
        Task<ImmutableList<TodoTask>> UpdateAllCompletedStateAsync(bool completed);
        Task<bool> ClearAsync();
    }
}
