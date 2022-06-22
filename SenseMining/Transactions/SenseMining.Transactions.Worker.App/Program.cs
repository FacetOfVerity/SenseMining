using Common.Infrastructure.Extensions;
using SenseMining.Transactions.Application.Services;
using SenseMining.Transactions.Infrastructure.Database;
using SenseMining.Transactions.Infrastructure.Utils;
using SenseMining.Transactions.Worker.App;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<Worker>(); })
    .Build();

await host

    .MigrateDatabase<TransactionsDbContext>((context, services) =>
    {
        var service = services.GetService<ITransactionsService>();
        new TransactionsContextSeed(service).SimpleExample().Wait();
    })
    .RunAsync();