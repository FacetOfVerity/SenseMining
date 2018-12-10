using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SenseMining.Utils;

namespace SenseMining.Importer.GroceryMarket
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Запустить процесс заполнения?");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                ConfigureServices();
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

        private static void ConfigureServices()
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
                services.AddSingleton<TransactionsDbProcessor>();
                services.AddSingleton(new CancellationTokenSource());
                services.AddSingleton(new ImporterOptions
                {
#if DEBUG
                    ResourcesPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "Resources")
#endif
#if RELEASE
                ResourcesPath = "Resources"
#endif
                });

                services.AddScoped<CancellationTokenSource>();

                //MassTransit
                services.AddMassTransit();

                var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(configuration["RabbitMq:Host"]), hostConfigurator =>
                    {
                        hostConfigurator.Username(configuration["RabbitMq:UserName"]);
                        hostConfigurator.Password(configuration["RabbitMq:Password"]);
                        hostConfigurator.Heartbeat(10);
                    });

                });

                services.AddSingleton<IPublishEndpoint>(bus);

                services.AddSingleton<IBus>(bus);
            });
        }
    }
}
