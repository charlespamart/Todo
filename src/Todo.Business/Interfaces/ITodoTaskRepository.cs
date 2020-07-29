using System;
using Todo.Domain.Models;

namespace Todo.Interfaces
{
    public interface ITodoTaskRepository
    {
        TodoTaskData[] GetTodoTasks();
        TodoTaskData GetTodoTask(Guid id);
        void Add(TodoTaskData TodoTask);
        void Remove(TodoTaskData todoTask);
        void Update(TodoTaskData TodoTask);
        void Clear();
    }
}
