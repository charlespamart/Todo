using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Todo.DAL.Models;
using Todo.Domain.Models;
using Xunit;

namespace Todo.DAL.Tests
{
    public class TodoTaskRepositoryShould
    {
        public static readonly Guid id = Guid.NewGuid();

        [Fact]
        public async Task GetAllTodoTasks()
        {
            var options = CreateInMemoryDB(nameof(GetAllTodoTasks));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var result = await repository.GetTodoTasksAsync();

            Assert.Equal(fixture.Context.TodoTasks.Select(x => x.ToDomain()), result);
        }

        [Fact]
        public async Task ReturnAnEmptyCollectionAfterClear()
        {
            var options = CreateInMemoryDB(nameof(ReturnAnEmptyCollectionAfterClear));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            await repository.ClearAsync();
            var result = await repository.GetTodoTasksAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetATodoTaskById()
        {
            var options = CreateInMemoryDB(nameof(GetATodoTaskById));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var expected = new TodoTask(id, "Never gonna say goodbye", false, 4);

            var result = await repository.GetTodoTaskAsync(id);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task ReturnNullIfTheTodoTaskIsNotFoundWhenCallingGetTodoTaskAsync()
        {
            var options = CreateInMemoryDB(nameof(ReturnNullIfTheTodoTaskIsNotFoundWhenCallingGetTodoTaskAsync));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var expected = new TodoTask(id, "Never gonna say goodbye", false, 4);

            var result = await repository.GetTodoTaskAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task AddATodoTaskAndReturnTheAddedTodoTask()
        {
            var options = CreateInMemoryDB(nameof(AddATodoTaskAndReturnTheAddedTodoTask));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            const string title = "Never gonna say goodbye";
            const int order = 4;
            var returnValue = await repository.AddAsync(title, order);

            Assert.Equal(title, returnValue.Title);
            Assert.False(returnValue.Completed);
            Assert.Equal(order, returnValue.Order);

            var newlyCreatedTodo = await repository.GetTodoTaskAsync(returnValue.Id);

            Assert.Equal(returnValue, newlyCreatedTodo);
        }

        [Fact]
        public async Task ClearAllTodoTasks()
        {
            var options = CreateInMemoryDB(nameof(ClearAllTodoTasks));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            await repository.ClearAsync();

            var result = await repository.GetTodoTasksAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnTrueWhenTodoTaskIsSuccesfullyRemoved()
        {
            var options = CreateInMemoryDB(nameof(ReturnTrueWhenTodoTaskIsSuccesfullyRemoved));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var result = await repository.RemoveAsync(id);

            Assert.True(result);
        }

        [Fact]
        public async Task ReturnFalseWhenTodoTaskCannotBeRemoved()
        {
            var options = CreateInMemoryDB(nameof(ReturnFalseWhenTodoTaskCannotBeRemoved));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var result = await repository.RemoveAsync(Guid.NewGuid());

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateTodoTaskAndReturnUpdatedTodoTask()
        {
            var options = CreateInMemoryDB(nameof(UpdateTodoTaskAndReturnUpdatedTodoTask));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var newBoolean = true;
            var newTitle = "https://www.youtube.com/watch?v=hq6BF8qoauM";
            var newOrder = 15;

            var result = await repository.UpdateAsync(id, newTitle, newBoolean, newOrder);

            Assert.Equal(newTitle, result.Title);
            Assert.Equal(newBoolean, result.Completed);
            Assert.Equal(newOrder, result.Order);
        }

        [Fact]
        public async Task ReturnNullIfTheTodoTaskIsNotFoundwhenCallingUpdate()
        {
            var options = CreateInMemoryDB(nameof(ReturnNullIfTheTodoTaskIsNotFoundwhenCallingUpdate));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var result = await repository.GetTodoTaskAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        private DbContextOptions<TodoTaskContext> CreateInMemoryDB(string dbName)
        {
            return new DbContextOptionsBuilder<TodoTaskContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }
    }
    class TodoContextFixture : IAsyncDisposable
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
            Context.TodoTasks.Add(new TodoTaskData() { Id = TodoTaskRepositoryShould.id, Title = "Never gonna say goodbye", Completed = false, Order = 4 });

            Context.SaveChanges();
        }

        public async ValueTask DisposeAsync()
        {
            await Context.Database.EnsureDeletedAsync();
        }
    }
}
