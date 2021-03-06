﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SenseMining.Database;
using SenseMining.Domain.Utils;
using SenseMining.Utils.AspNetCore.Mvc;
using SenseMining.Worker;

namespace SenseMining.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDatabase<DatabaseContext>()
                .SetUpWithService<DataInitializer>(a => a.Initialize().Wait())
                //.SetUpWithService<FpTreeRelevanceWorker>(a => a.Run())
                .Run();

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
