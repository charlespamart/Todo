

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Todo.Interfaces;

namespace Todo
{
    public class Startup
    {
        private readonly string _myCorsPolicyAllowAll = "CorsPolicyAllowAll";

        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
            services.AddDbContext<TodoTaskContext>(opt =>
                opt.UseInMemoryDatabase("TodoDB"));

            services.AddCors(options =>
            {
                options.AddPolicy(name: _myCorsPolicyAllowAll,
                              builder =>
                              {
                                  builder.AllowAnyOrigin();
                                  builder.AllowAnyMethod();
                                  builder.AllowAnyHeader();
                              });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TodoTaskContext todoContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(_myCorsPolicyAllowAll);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
