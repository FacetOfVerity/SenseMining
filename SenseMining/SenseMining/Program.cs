using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SenseMining.Database;
using SenseMining.Utils.AspNetCore.Mvc;

namespace SenseMining.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDatabase<DatabaseContext>()
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
