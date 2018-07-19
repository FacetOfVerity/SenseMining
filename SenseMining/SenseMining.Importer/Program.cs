using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SenseMining.Database;
using SenseMining.Domain.Extensions;
using SenseMining.Utils;

namespace SenseMining.Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new ConnectionOptions
            {
                ConnectionString = "mongodb://localhost",
                DatabaseName = "fiscalDb",
                FiscalDataCollectionName = "transactions"
            };

            Console.WriteLine("Подклчение к БД имеет вид:");
            Console.Write(options.ToString());
            Console.WriteLine("Запустить процесс заполнения?");

            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                ConfigureServices(options);
                var processor = DiConfigurator.ServiceProvider.GetService<TransactionsDbProcessor>();
                Task.WaitAny(processor.Start(), Task.Run(() =>
                {
                    Console.WriteLine("Для отмены нажмите Esc");
                    var key = Console.ReadKey().Key;
                    if (key == ConsoleKey.Escape)
                    {
                        DiConfigurator.ServiceProvider.GetService<CancellationTokenSource>().Cancel();
                        Console.WriteLine("Операция отменена");
                    }
                }));
            }

            Console.ReadKey();
        }

        private static void ConfigureServices(ConnectionOptions options)
        {
            var builder = new ConfigurationBuilder()
#if DEBUG
                .SetBasePath(Path.Combine(Environment.CurrentDirectory, "..", "..", ".."))
#endif
#if RELEASE
               .SetBasePath(Environment.CurrentDirectory)
#endif
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            DiConfigurator.SetUp(services =>
            {
                services.AddSingleton(new MongoClient(options.ConnectionString));
                services.AddSingleton<IMongoDatabase>(a => a.GetService<MongoClient>().GetDatabase(options.DatabaseName));
                services.AddSingleton<TransactionsDbProcessor>();
                services.AddSingleton(new CancellationTokenSource());

                services.AddDomain();
                services.AddSingleton(options);

                //Entity framework
                services.AddDbContext<DatabaseContext>(a =>
                    {
                        a.UseNpgsql(configuration["ConnectionStrings:SenseMiningStore"]);
                    });
            });
        }
    }
}
