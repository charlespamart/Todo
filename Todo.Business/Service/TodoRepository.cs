using System;
using System.Linq;
using Todo.DAL;
using Todo.Domain.Models;
using Todo.Interfaces;

namespace Todo.Service
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _dbContext;

        public TodoRepository(TodoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TodoTaskData[]  GetTodoTasks()
        {
            return _dbContext.TodoTasks.ToArray();
        }
        public TodoTaskData GetTodoTask(Guid id)
        {
            return _dbContext.TodoTasks.Find(id);
        }
        public void Add(TodoTaskData TodoTask)
        {
            _dbContext.TodoTasks.Add(TodoTask);
            _dbContext.SaveChanges();
        }

        public void Remove(TodoTaskData todoTask)
        {
            _dbContext.TodoTasks.Remove(todoTask);
            _dbContext.SaveChanges();
        }
        public void Update(TodoTaskData todoTask)
        {
            _dbContext.Update(todoTask);
            _dbContext.SaveChanges();
        }
        public void Clear()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
