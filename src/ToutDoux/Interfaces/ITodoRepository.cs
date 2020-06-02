﻿using System.Collections.Generic;
using Todo.Models;

namespace Todo.Interfaces
{
    public interface ITodoRepository
    {
        TodoTask[] GetTodoTasks();
        TodoTask GetTodoTask(long id);
        void Add(TodoTask TodoTask);
        void Remove(TodoTask TodoTask);
        void RemoveAll();
    }
}