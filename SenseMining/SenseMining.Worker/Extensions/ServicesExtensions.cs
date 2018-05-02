using Microsoft.Extensions.DependencyInjection;

namespace SenseMining.Worker.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddFpTreeJobs(this IServiceCollection services)
        {
            services.AddSingleton<FpTreeRelevanceWorker>();
            services.AddScoped<FpTreeRelevanceJob>();

            return services;
        }
    }
}
