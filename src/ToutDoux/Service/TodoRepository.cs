using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Interfaces;
using Todo.Models;

namespace Todo.Service
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _dbContext;

        public TodoRepository(TodoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TodoTask[]  GetTodoTasks()
        {
            return _dbContext.TodoTasks.ToArray();
        }
        public TodoTask GetTodoTask(long id)
        {
            return _dbContext.TodoTasks.Find(id);
        }
        public void Add(TodoTask TodoTask)
        {
            _dbContext.TodoTasks.Add(TodoTask);
            _dbContext.SaveChanges();
        }

        public void Remove(TodoTask TodoTask)
        {
            _dbContext.TodoTasks.Remove(TodoTask);
            _dbContext.SaveChanges();
        }
        public TodoTask Update(long id, TodoTask TodoTask)
        {
            TodoTask todoTaskToUpdate = _dbContext.TodoTasks.Find(id);

            todoTaskToUpdate.Completed = TodoTask.Completed;
            todoTaskToUpdate.Order = TodoTask.Order;
            todoTaskToUpdate.Title = TodoTask.Title;

            _dbContext.SaveChanges();
            return todoTaskToUpdate;
        }
        public void Clear()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
