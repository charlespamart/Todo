
using System;
using Todo.DAL;
using Todo.DAL.Models;

namespace Todo.API.IntegrationTests
{
    class TestData
    {
        public static void PopulateDB(TodoTaskContext todoTaskContext)
        {
            todoTaskContext.TodoTasks.Add(new TodoTaskData() { Id = new Guid("00000000-0000-0000-0000-000000000001"), Title = "", Completed = false, Order = 0 });
            todoTaskContext.TodoTasks.Add(new TodoTaskData() { Id = new Guid("00000000-0000-0000-0000-000000000002"), Title = "", Completed = false, Order = 0 });
            todoTaskContext.TodoTasks.Add(new TodoTaskData() { Id = new Guid("00000000-0000-0000-0000-000000000003"), Title = "", Completed = false, Order = 0 });
            todoTaskContext.TodoTasks.Add(new TodoTaskData() { Id = new Guid("00000000-0000-0000-0000-000000000004"), Title = "", Completed = false, Order = 0 });
            todoTaskContext.TodoTasks.Add(new TodoTaskData() { Id = new Guid("00000000-0000-0000-0000-000000000005"), Title = "", Completed = false, Order = 0 });
            todoTaskContext.SaveChanges();
        }
    }
}
