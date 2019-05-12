using System;
using System.IO;
using System.Threading;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SenseMining.Database;
using SenseMining.Domain.Extensions;
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
                var service = scope.ServiceProvider.GetService<IHostedService>();
                var token = scope.ServiceProvider.GetService<CancellationTokenSource>();
                service.StartAsync(token.Token).Wait(token.Token);

                Console.WriteLine("Для остановки нажмите любую клавишу");
                Console.ReadKey();
                token.Cancel();
            }
           
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
                services.AddDomain();


                //Entity framework
                services.AddDbContext<DatabaseContext>(a =>
                {
                    a.UseNpgsql(configuration["ConnectionStrings:SenseMiningStore"]);
                });

                services.AddScoped<CancellationTokenSource>();

                //MassTransit
                services.AddScoped<TransactionsQueueConsumer>();
                services.AddMassTransit(c =>
                {
                    c.AddConsumer<TransactionsQueueConsumer>();
                });

                services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(
                    cfg =>
                    {
                       var host = cfg.Host(new Uri(configuration["RabbitMq:Host"]), hostConfigurator =>
                        {
                            hostConfigurator.Username(configuration["RabbitMq:UserName"]);
                            hostConfigurator.Password(configuration["RabbitMq:Password"]);
                            hostConfigurator.Heartbeat(10);
                        });

                        cfg.ReceiveEndpoint(host, e =>
                        {
                            e.PrefetchCount = 16;
                            e.UseMessageRetry(x => x.Interval(2, 100));
                            e.Durable = false;

                            e.LoadFrom(provider);
                            EndpointConvention.Map<TransactionsQueueConsumer>(e.InputAddress);
                        });
                    }));

                services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
                services.AddSingleton<IHostedService, BusService>();
            });
        }
    }
}
