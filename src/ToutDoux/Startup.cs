using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToutDoux.Models;

namespace ToutDoux
{
    public class Startup
    {
        readonly string MyCorsPolicyAllowAll = nameof(MyCorsPolicyAllowAll);
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ToutDouxContext>(opt =>
                opt.UseInMemoryDatabase("ToutDouxDB"));

            services.AddCors(options => {
                options.AddPolicy(name: MyCorsPolicyAllowAll,
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

            app.UseCors(MyCorsPolicyAllowAll);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync("Hello there.");
                });
            });

        }
    }
}
