using Microsoft.Extensions.DependencyInjection;
using SenseMining.Domain.Services;
using SenseMining.Domain.TransactionsProcessing;

namespace SenseMining.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<ITransactionsConsumer, TransactionsConsumer>();
            services.AddScoped<IFpTreeService, FpTreeService>();

            return services;
        }
    }
}
