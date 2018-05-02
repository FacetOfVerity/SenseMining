using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SenseMining.Database;
using SenseMining.Domain.Extensions;
using SenseMining.Utils.AspNetCore.Mvc.Filters;
using SenseMining.Worker.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace SenseMining.API
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Environment = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            //MVC
            services.AddMvc(options =>
                {
                    options.Filters.AddService(typeof(ExceptionFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            services.AddScoped<ExceptionFilter>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Version = "v1", Title = "API", Description = "Service API" });
                options.DescribeAllEnumsAsStrings();
            });

            //Entity framework
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(Configuration["ConnectionStrings:SenseMiningStore"],
                    o => o.MigrationsAssembly("SenseMining.API"));
            });

            services.AddScoped(a =>
            {
                var accessor = a.GetService<IHttpContextAccessor>();
                if (accessor.HttpContext == null)
                    return new CancellationTokenSource();
                return CancellationTokenSource.CreateLinkedTokenSource(
                    accessor.HttpContext.RequestAborted);
            });

            services.AddDomain();
            services.AddFpTreeJobs();
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Service API");
                options.RoutePrefix = "help";
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
