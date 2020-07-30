using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Business.Interfaces
{
    class ITodoService
    {
        List<TodoTask> GetTodoTasks();
        TodoTask GetTodoTask(Guid id);
        void Add(TodoTask TodoTask);
        void Remove(TodoTask todoTask);
        void Update(TodoTask TodoTask);
        void Clear();
    }
}
