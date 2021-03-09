using Microsoft.EntityFrameworkCore;
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

        public async Task<IImmutableList<TodoTask>> GetAllAsync()
        {
            return await Task.FromResult(_dbContext.TodoTasks.Select(x => x.ToDomain()).ToImmutableList());
        }

        public async Task<TodoTask> GetByIdAsync(Guid id)
        {
            var todoTaskData = await _dbContext.TodoTasks.SingleOrDefaultAsync(x => x.Id == id);
            if (todoTaskData == null)
                return null;
            return todoTaskData.ToDomain();
        }

        public async Task<TodoTask> AddAsync(string title, int order)
        {
            var todoTaskData = new TodoTaskData { Title = title, Order = order };

            try
            {
                await _dbContext.TodoTasks.AddAsync(todoTaskData);
                var writtenStateEntries = await _dbContext.SaveChangesAsync();

                if (writtenStateEntries == 1)
                {
                    return todoTaskData.ToDomain();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var todoTaskData = await _dbContext.TodoTasks.SingleOrDefaultAsync(x => x.Id == id);

            if (todoTaskData == null)
            {
                return false;
            }

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

            if (completed.HasValue)
            {
                todoTaskDataToUpdate.Completed = completed.Value;
            }

            if (order.HasValue)
            {
                todoTaskDataToUpdate.Order = order.Value;
            }

            _dbContext.Update(todoTaskDataToUpdate);
            var writtenStateEntries = await _dbContext.SaveChangesAsync();

            if (writtenStateEntries == 1)
            {
                return todoTaskDataToUpdate.ToDomain();
            }
            return null;
        }

        public async Task<IImmutableList<TodoTask>> UpdateAllCompletedStateAsync(bool completed)
        {
            var todoTasks = _dbContext.TodoTasks;

            foreach(TodoTaskData todoTask in todoTasks)
            {
                todoTask.Completed = completed;
            }

            _dbContext.UpdateRange(todoTasks);
            var writtenStateEntries = await _dbContext.SaveChangesAsync();

            if (writtenStateEntries == todoTasks.Count())
            {
                return todoTasks.Select(x => x.ToDomain()).ToImmutableList();
            }
            return null;
        }

        public async Task<bool> ClearAsync()
        {
            var todoTasks = _dbContext.TodoTasks;
            _dbContext.TodoTasks.RemoveRange(todoTasks);
            var writtenStateEntries = await _dbContext.SaveChangesAsync();

            return writtenStateEntries != 0;
        }

        public async Task<ImmutableList<TodoTask>> ClearAllCompletedAsync()
        {
            var todoTasks = _dbContext.TodoTasks;
            var todoTasksToRemove = todoTasks.Where(x => x.Completed == true).ToList();

            _dbContext.TodoTasks.RemoveRange(todoTasksToRemove);
            var writtenStateEntries = await _dbContext.SaveChangesAsync();

            if (writtenStateEntries == todoTasksToRemove.Count)
            {
                return todoTasks.Where(x => x.Completed == false).Select(x => x.ToDomain()).ToImmutableList();
            }
            return null;
        }
    }
}
