using Microsoft.Extensions.DependencyInjection;
using SenseMining.FPG.Application.Services.FpTree;

namespace SenseMining.FPG.Application.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IFpTreeProvider, FpTreeProvider>();

        return services;
    }
}