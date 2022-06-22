using SenseMining.FPG.API.Filters;

namespace SenseMining.FPG.API;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        //MVC
        services.AddMvc(options =>
        {
            options.Filters.AddService(typeof(ExceptionFilter));
        });
        // .AddJsonOptions(options =>
        // {
        //     options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; //TODO проверить
        // });
        services.AddScoped<ExceptionFilter>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Info { Version = "v1", Title = "API", Description = "Service API" });
            options.DescribeAllEnumsAsStrings();
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SenseMining.FPG.API.xml"));
        });

        //Entity framework
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(Configuration["ConnectionStrings:SenseMiningStore"],
                o => o.MigrationsAssembly("SenseMining.FPG.API"));
        });

        services.AddScoped(a =>
        {
            var accessor = a.GetService<IHttpContextAccessor>();
            if (accessor.HttpContext == null)
                return new CancellationTokenSource();
            return CancellationTokenSource.CreateLinkedTokenSource(
                accessor.HttpContext.RequestAborted);
        });

        services.AddAutoMapper(a =>
        {
            a.AddProfile(typeof(DomainMappingProfile));
        });

        services.AddDomain();
        services.AddFpTreeJobs();
        services.AddScoped<DataInitializer>();
    }
        
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseStaticFiles();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Service API");
            options.RoutePrefix = "help";
        });
            
        app.UseRouting();

        app.UseEndpoints(_ => {});
    }
}