using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Todo.API.IntegrationTests
{
    public class TodoWebApiIntegrationsTests
    {
        private const string BaseUri = "https://localhost:5001/todotasks/";
        private readonly HttpClient _client;

        public TodoWebApiIntegrationsTests(TodoWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CanGetAllTodoTasksAync()
        {
            using var fixture = new TodoWebApplicationFactory<Startup>();
            var response = await _client.GetAsync(BaseUri);

            Assert.Equal(HttpStatusCode.OK, HttpStatusCode.OK);
        }
    }
}
