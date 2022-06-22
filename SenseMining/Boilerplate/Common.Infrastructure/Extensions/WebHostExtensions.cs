using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Common.Infrastructure.Extensions;

/// <summary>
/// Extensions for <see cref="IHost"/>
/// <remarks>Taken from eShopOnContainers</remarks>
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Execute some operation at application launch.
    /// </summary>
    /// <typeparam name="TService">Service type.</typeparam>
    /// <param name="host"><see cref="IHost"/>.</param>
    /// <param name="setUpAction">Operation delegate.</param>
    public static IHost SetUpWithService<TService>(this IHost host, Action<TService> setUpAction)
    {
        using (var scope = host.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<TService>();
            setUpAction(service);
        }

        return host;
    }

    /// <summary>
    /// Checks if application run under kubernetes.
    /// </summary>
    /// <param name="host"><see cref="IHost"/>.</param>
    public static bool IsInKubernetes(this IHost host)
    {
        var cfg = host.Services.GetService<IConfiguration>();
        var orchestratorType = cfg.GetValue<string>("OrchestratorType");
        return orchestratorType?.ToUpper() == "K8S";
    }

    /// <summary>
    /// Executes database migration.
    /// </summary>
    /// <param name="host"><see cref="IHost"/>.</param>
    /// <param name="seeder">Seed data delegate.</param>
    /// <typeparam name="TContext">Database context type.</typeparam>
    /// <remarks>This code is taken from eShopOnContainers repo.</remarks>
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider>? seeder = null)
        where TContext : DbContext
    {
        var underK8s = host.IsInKubernetes();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}",
                    typeof(TContext).Name);

                if (underK8s)
                {
                    InvokeSeeder(seeder, context, services);
                }
                else
                {
                    var retries = 10;
                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(
                            retryCount: retries,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                            onRetry: (exception, timeSpan, retry, ctx) =>
                            {
                                logger.LogWarning(exception,
                                    "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                                    nameof(TContext), exception.GetType().Name, exception.Message, retry, retries);
                            });

                    //if the sql server container is not created on run docker compose this
                    //migration can't fail for network related exception. The retry options for DbContext only 
                    //apply to transient exceptions
                    // Note that this is NOT applied when running some orchestrators (let the orchestrator to recreate the failing service)
                    retry.Execute(() => InvokeSeeder(seeder, context, services));
                }

                logger.LogInformation("Migrated database associated with context {DbContextName}",
                    typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "An error occurred while migrating the database used on context {DbContextName}",
                    typeof(TContext).Name);
                if (underK8s)
                {
                    throw; // Rethrow under k8s because we rely on k8s to re-run the pod
                }
            }
        }

        return host;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider>? seeder, TContext context,
        IServiceProvider services)
        where TContext : DbContext
    {
        context.Database.Migrate();
        if (seeder is not null)
        {
            seeder(context, services);
        }
    }
}