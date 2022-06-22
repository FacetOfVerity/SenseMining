using Common.Infrastructure.Extensions;
using SenseMining.FPG.Infrastructure.Database;

namespace SenseMining.FPG.API;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
            .Build()
            .MigrateDatabase<FpgDbContext>()
            .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                
            });
}