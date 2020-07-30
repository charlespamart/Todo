using System;
using System.Collections.Generic;
using Todo.DAL.Models;

namespace Todo.Repository.Interfaces
{
    public interface ITodoTaskRepository
    {
        List<TodoTaskData> GetTodoTasks();
        TodoTaskData GetTodoTask(Guid id);
        void Add(TodoTaskData TodoTask);
        void Remove(TodoTaskData todoTask);
        void Update(TodoTaskData TodoTask);
        void Clear();
    }
}
