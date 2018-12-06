using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
            });
        }
    }
}
