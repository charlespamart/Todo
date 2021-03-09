using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Todo.API.Configuration;
using Todo.DAL;
using Todo.Domain;
using Todo.Domain.Interfaces;

namespace Todo
{
    public class Startup
    {
        private const string MyCorsPolicyAllowAll = "CorsPolicyAllowAll";
        private const string DBName = "TodoTaskDB";

        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson().AddJsonOptions(options => {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
            services.AddScoped<ITodoTaskService, TodoTaskService>();

            services.AddDbContext<TodoTaskContext>(opt =>
                opt.UseInMemoryDatabase(DBName));

            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                opt.ErrorResponses = new ApiVersioningCustomError();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyCorsPolicyAllowAll,
                              builder =>
                              {
                                  builder.AllowAnyOrigin();
                                  builder.AllowAnyMethod();
                                  builder.AllowAnyHeader();
                              });
            });

            services.AddSwaggerDocument();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(MyCorsPolicyAllowAll);
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
