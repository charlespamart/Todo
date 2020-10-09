using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Todo.Domain.TestHelpers;
using Xunit;

namespace Todo.DAL.Tests
{
    public class TodoTaskRepositoryShould
    {

        [Fact]
        public async Task GetAllTodoTasks()
        {
            var options = CreateInMemoryDB(nameof(GetAllTodoTasks));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var result = await repository.GetAllAsync();

            Assert.Equal(fixture.Context.TodoTasks.Select(x => x.ToDomain()), result);
        }

        [Fact]
        public async Task ReturnAnEmptyCollectionAfterClear()
        {
            var options = CreateInMemoryDB(nameof(ReturnAnEmptyCollectionAfterClear));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            await repository.ClearAsync();
            var result = await repository.GetAllAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetATodoTaskById()
        {
            var options = CreateInMemoryDB(nameof(GetATodoTaskById));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var expected = TestHelpers.todoTask;

            var result = await repository.GetByIdAsync(TestHelpers.Guid);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task ReturnNullIfTheTodoTaskIsNotFoundWhenCallingGetTodoTaskAsync()
        {
            var options = CreateInMemoryDB(nameof(ReturnNullIfTheTodoTaskIsNotFoundWhenCallingGetTodoTaskAsync));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task AddATodoTaskAndReturnTheAddedTodoTask()
        {
            var options = CreateInMemoryDB(nameof(AddATodoTaskAndReturnTheAddedTodoTask));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            const string title = "Todotask";
            const int order = 4;

            var returnValue = await repository.AddAsync(title, order);

            Assert.Equal(title, returnValue.Title);
            Assert.False(returnValue.Completed);
            Assert.Equal(order, returnValue.Order);

            var newlyCreatedTodo = await repository.GetByIdAsync(returnValue.Id);

            Assert.Equal(returnValue, newlyCreatedTodo);
        }

        [Fact]
        public async Task ClearAllTodoTasks()
        {
            var options = CreateInMemoryDB(nameof(ClearAllTodoTasks));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var result = await repository.ClearAsync();

            Assert.True(result);
        }

        [Fact]
        public async Task ClearAllCompletedTask()
        {
            var options = CreateInMemoryDB(nameof(ClearAllTodoTasks));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);
            var expected = TestHelpers.TodoTasks.Where(x => x.Completed == false);

            var result = await repository.ClearAllCompletedAsync();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task ReturnTrueWhenTodoTaskIsSuccesfullyRemoved()
        {
            var options = CreateInMemoryDB(nameof(ReturnTrueWhenTodoTaskIsSuccesfullyRemoved));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var result = await repository.RemoveAsync(TestHelpers.Guid);

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
        public async Task UpdateAllTodosCompletedStateAndReturnResult()
        {
            var options = CreateInMemoryDB(nameof(UpdateAllTodosCompletedStateAndReturnResult));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            var result = await repository.UpdateAllCompletedStateAsync(true);

            Assert.NotEmpty(result);
            Assert.Equal(TestHelpers.TodoTasksCompleted, result);
        }

        [Fact]
        public async Task UpdateTodoTaskAndReturnUpdatedTodoTask()
        {
            var options = CreateInMemoryDB(nameof(UpdateTodoTaskAndReturnUpdatedTodoTask));

            await using var fixture = new TodoContextFixture(new TodoTaskContext(options));
            var repository = new TodoTaskRepository(fixture.Context);

            const bool newBoolean = true;
            const string newTitle = "Title";
            const int newOrder = 15;

            var result = await repository.UpdateAsync(TestHelpers.Guid, newTitle, newBoolean, newOrder);

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

            var result = await repository.GetByIdAsync(Guid.NewGuid());

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
            Context.Database.EnsureCreatedAsync();

            Context.TodoTasks.AddRangeAsync(TestHelpers.TodoTasks.Select(TestHelpers.TodoDataFromDomain).ToImmutableList());

            Context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await Context.Database.EnsureDeletedAsync();
        }
    }
}
