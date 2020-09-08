using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Todo.DAL.Models;
using Todo.Domain.Models;

namespace Todo.Domain.TestHelpers
{
    public static class TestHelpers
    {
        public static readonly Guid Guid = new Guid("00000000-0000-0000-0000-000000000001");

        public static TodoTask todoTask = new TodoTask (Guid, "Todotask 0", true, 0);

        public static readonly IImmutableList<TodoTask> TodoTasks = new List<TodoTask>
            {
                new TodoTask(Guid, "Todotask 0", true, 0),
                new TodoTask(new Guid("00000000-0000-0000-0000-000000000002"), "Todotask 1", false, 1),
                new TodoTask(new Guid("00000000-0000-0000-0000-000000000003"), "Todotask 2", true, 2),
                new TodoTask(new Guid("00000000-0000-0000-0000-000000000004"), "Todotask 3", false, 3),
            }.ToImmutableList();

        public static TodoTaskData TodoDataFromDomain(TodoTask todoTask)
        {
            return new TodoTaskData {
                Id = todoTask.Id,
                Title = todoTask.Title,
                Completed = todoTask.Completed,
                Order = todoTask.Order
            };
        }
    }
}
