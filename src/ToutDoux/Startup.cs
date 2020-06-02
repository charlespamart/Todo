

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Todo.Interfaces;
using Todo.Models;
using Todo.Service;

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
            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddScoped<ITodoRepository, TodoRepository>();

            services.AddDbContext<TodoContext>(opt =>
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
