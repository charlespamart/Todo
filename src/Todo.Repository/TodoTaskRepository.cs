using System;
using System.Collections.Generic;
using System.Linq;
using Todo.DAL;
using Todo.DAL.Models;
using Todo.Repository.Interfaces;

namespace Todo.Service
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly TodoTaskContext _dbContext;

        public TodoTaskRepository(TodoTaskContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TodoTaskData>  GetTodoTasks()
        {
            return _dbContext.TodoTasks.ToList();
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
