using Microsoft.EntityFrameworkCore;
using System;
using Todo.DAL;
using Todo.Domain.Models;
using Todo.Service;
using Xunit;

namespace Todo.Tests.Services
{
    public class TodoTaskRepositoryShould
    {
        [Fact]
        public void GetAllTodoTasks()
        {
            var options = new DbContextOptionsBuilder<TodoTaskContext>()
                .UseInMemoryDatabase(databaseName: nameof(GetAllTodoTasks))
                .Options;

            using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var result = repository.GetTodoTasks();

            Assert.Equal(fixture.Context.TodoTasks, result);
        }
        [Fact]
        public void GetATodoTaskById()
        {
            var options = new DbContextOptionsBuilder<TodoTaskContext>()
                .UseInMemoryDatabase(databaseName: nameof(GetATodoTaskById))
                .Options;

            using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var expected = new TodoTaskData() { Id = Guid.Parse("ac7ef91f-221e-4d9c-868d-c65a3ffc92c8"), Title = "Gravé dans la roche", Completed = false, Order = 4 };

            var result = repository.GetTodoTask(Guid.Parse("ac7ef91f-221e-4d9c-868d-c65a3ffc92c8"));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ClearAllTodoTasks()
        {
            var options = new DbContextOptionsBuilder<TodoTaskContext>()
                .UseInMemoryDatabase(databaseName: nameof(ClearAllTodoTasks))
                .Options;

            using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            repository.Clear();

            var result = repository.GetTodoTasks();

            Assert.Empty(result);
        }

        [Fact]
        public void RemoveTodoTask()
        {
            var options = new DbContextOptionsBuilder<TodoTaskContext>()
                .UseInMemoryDatabase(databaseName: nameof(RemoveTodoTask))
                .Options;

            using (var fixture = new TodoContextFixture(new TodoTaskContext(options)))
            {
                var repository = new TodoTaskRepository(fixture.Context);
                var toRemove = repository.GetTodoTask(Guid.Parse("ac7ef91f-221e-4d9c-868d-c65a3ffc92c8"));

                repository.Remove(toRemove);

                var result = repository.GetTodoTask(Guid.Parse("ac7ef91f-221e-4d9c-868d-c65a3ffc92c8"));

                Assert.Null(result);
            }
        }

        [Fact]
        public void UpdateTodoTask()
        {
            var options = new DbContextOptionsBuilder<TodoTaskContext>()
                .UseInMemoryDatabase(databaseName: nameof(RemoveTodoTask))
                .Options;

            using (var fixture = new TodoContextFixture(new TodoTaskContext(options)))
            {
                var repository = new TodoTaskRepository(fixture.Context);
                var expected = repository.GetTodoTask(Guid.Parse("ac7ef91f-221e-4d9c-868d-c65a3ffc92c8"));

                expected.Completed = true;
                expected.Title = "https://www.youtube.com/watch?v=hq6BF8qoauM";
                expected.Order = 15;

                repository.Update(expected);

                var result = repository.GetTodoTask(Guid.Parse("ac7ef91f-221e-4d9c-868d-c65a3ffc92c8"));

                Assert.Equal(expected, result);
            }
        }
    }

    class TodoContextFixture : IDisposable
    {
        public TodoTaskContext Context { get; }

        public TodoContextFixture(TodoTaskContext context)
        {
            Context = context;
            Seed();
        }

        private void Seed()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();

            Context.TodoTasks.Add(new TodoTaskData() { Title = "Never gonna give you up", Completed = false, Order = 0 });
            Context.TodoTasks.Add(new TodoTaskData() { Title = "Never gonna let you down", Completed = false, Order = 1 });
            Context.TodoTasks.Add(new TodoTaskData() { Title = "Never gonna run around and desert you", Completed = false, Order = 2 });
            Context.TodoTasks.Add(new TodoTaskData() { Title = "Never gonna make you cry", Completed = false, Order = 3 });
            Context.TodoTasks.Add(new TodoTaskData() { Id = Guid.Parse("ac7ef91f-221e-4d9c-868d-c65a3ffc92c8"), Title = "Gravé dans la roche", Completed = false, Order = 4 });

            Context.SaveChanges();
        }


        public void Dispose()
        {
            Context.Database.EnsureDeleted();
        }
    }
}
