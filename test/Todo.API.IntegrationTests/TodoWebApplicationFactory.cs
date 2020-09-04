using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Todo.DAL;

namespace Todo.API.IntegrationTests
{
    public class TodoWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<TodoTaskContext>(options =>
                {
                    options.UseInMemoryDatabase("TodoTestDB");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var appDb = scopedServices.GetRequiredService<TodoTaskContext>();

                appDb.Database.EnsureCreated();

                try
                {
                    TestData.PopulateDB(appDb);
                }
                catch (Exception ex)
                {
                    // Replace with logger
                }
            });
        }
    }
}
