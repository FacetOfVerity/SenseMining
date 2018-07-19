using System;
using Microsoft.Extensions.DependencyInjection;
using SenseMining.Domain.Extensions;
using SenseMining.Listener.RabiitMq;
using SenseMining.Utils;

namespace SenseMining.Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureServices();
            using (var scope = DiConfigurator.ServiceProvider.CreateScope())
            {
                var listener = scope.ServiceProvider.GetService<TransactionsQueueListener>();
                listener.StartListen();

                Console.ReadLine();
            }
        }

        private static void ConfigureServices()
        {
            DiConfigurator.SetUp(services =>
            {
                services.AddDomain();
                services.AddScoped<TransactionsQueueListener>();
                services.AddSingleton(new RabbitMqOptions
                {
                    HostName = "localhost",
                    ExchangeName = "SenseMining",
                    QueueName = "SenseMining_Transactions",
                    Password = "guest",
                    UserName = "guest"
                });
            });
        }
    }
}
