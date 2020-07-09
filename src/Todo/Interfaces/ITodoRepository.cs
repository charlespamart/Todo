using System.Collections.Generic;
using Todo.Models;

namespace Todo.Interfaces
{
    public interface ITodoRepository
    {
        TodoTask[] GetTodoTasks();
        TodoTask GetTodoTask(long id);
        void Add(TodoTask TodoTask);
        void Remove(long id);
        TodoTask Update(long i, TodoTask TodoTask);
        void Clear();
    }
}
