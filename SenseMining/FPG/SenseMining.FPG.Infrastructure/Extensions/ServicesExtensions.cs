using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SenseMining.FPG.Application.Abstractions;
using SenseMining.FPG.Infrastructure.Database;

namespace SenseMining.FPG.Infrastructure.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FpgDbContext>(options =>
            options.UseNpgsql(configuration.GetSection("SenseMiningDb").Get<NpgsqlConnectionStringBuilder>()
                .ConnectionString));
        services.AddScoped<IFpgDbContext, FpgDbContext>();

        return services;
    }
}