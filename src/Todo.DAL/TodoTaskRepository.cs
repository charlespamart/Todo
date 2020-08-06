﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Todo.DAL.Models;
using Todo.Domain.Interfaces;
using Todo.Domain.Models;

namespace Todo.DAL
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly TodoTaskContext _dbContext;

        public TodoTaskRepository(TodoTaskContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IImmutableList<TodoTask>> GetTodoTasksAsync()
        {
            return await Task.FromResult(_dbContext.TodoTasks.Select(x => x.ToDomain()).ToImmutableList());
        }

        public async Task<TodoTask> GetTodoTaskAsync(Guid id)
        {
            var todoTaskData = await _dbContext.TodoTasks.SingleOrDefaultAsync(x => x.Id == id);
            if (todoTaskData == null)
                return null;
            return todoTaskData.ToDomain();
        }

        public async Task<TodoTask> AddAsync(string title, int order)
        {
            var todoTaskData = new TodoTaskData { Title = title, Order = order};

            try
            {
                await _dbContext.TodoTasks.AddAsync(todoTaskData);
                await _dbContext.SaveChangesAsync();
                return todoTaskData.ToDomain();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            //var todoTaskData = new TodoTaskData { Id = id };
            var todoTaskData = await _dbContext.TodoTasks.SingleOrDefaultAsync(x => x.Id == id);
            try
            {
                _dbContext.TodoTasks.Remove(todoTaskData);
                return await _dbContext.SaveChangesAsync() == 1;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<TodoTask> UpdateAsync(Guid id, string title, bool? completed, int? order)
        {
            var todoTaskDataToUpdate = await _dbContext.TodoTasks.SingleOrDefaultAsync(x => x.Id == id);

            if (todoTaskDataToUpdate == null)
            {
                return null;
            }

            if (title != null)
            {
                todoTaskDataToUpdate.Title = title;
            }

            if(completed.HasValue)
            {
                todoTaskDataToUpdate.Completed = completed.Value;
            }

            if (order.HasValue)
            {
                todoTaskDataToUpdate.Order = order.Value;
            }

            _dbContext.Update(todoTaskDataToUpdate);
            await _dbContext.SaveChangesAsync();
            return todoTaskDataToUpdate.ToDomain();
        }
        public async Task ClearAsync()
        {
            var todoTasks = _dbContext.TodoTasks;
            _dbContext.TodoTasks.RemoveRange(todoTasks);
            await _dbContext.SaveChangesAsync();
        }
    }
}
