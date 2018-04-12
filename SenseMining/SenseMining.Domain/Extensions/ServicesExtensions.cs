using Microsoft.Extensions.DependencyInjection;

namespace SenseMining.Domain.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<IFrequenciesService, FrequenciesService>();
            services.AddScoped<IProductsService, ProductsService>();

            return services;
        }
    }
}
