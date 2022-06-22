using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SenseMining.Transactions.Application.Abstractions;
using SenseMining.Transactions.Infrastructure.Database;

namespace SenseMining.Transactions.Infrastructure.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransactionsDbContext>(options =>
            options.UseNpgsql(configuration.GetSection("SenseMiningDb").Get<NpgsqlConnectionStringBuilder>()
                .ConnectionString));
        services.AddScoped<ITransactionsDbContext, TransactionsDbContext>();

        return services;
    }
}