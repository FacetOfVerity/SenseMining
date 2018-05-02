using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SenseMining.Utils.AspNetCore.Mvc
{
    /// <summary>
    /// Методы расширения для <see cref="IWebHost"/>
    /// </summary>
    public static class WebHostExtensions
    {
        /// <summary>
        /// Выполенение операции с сервисом при запуске приложения
        /// </summary>
        /// <typeparam name="TService">Тип сервиса</typeparam>
        /// <param name="host"><see cref="IWebHost"/></param>
        /// <param name="setUpAction">Делегат операции</param>
        public static IWebHost SetUpWithService<TService>(this IWebHost host, Action<TService> setUpAction)
        {
            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                setUpAction(service);
            }

            return host;
        }

        /// <summary>
        /// Выполенение операции с сервисом при запуске приложения
        /// </summary>
        /// <typeparam name="TService">Тип сервиса</typeparam>
        /// <param name="host"><see cref="IWebHost"/></param>
        /// <param name="setUpAction">Делегат операции</param>
        public static IWebHost SetUpWithSingleton<TService>(this IWebHost host, Action<TService> setUpAction)
        {
           

            return host;
        }

        /// <summary>
        /// Миграция БД
        /// </summary>
        /// <typeparam name="TContext">Тип контекста БД</typeparam>
        /// <param name="host"><see cref="IWebHost"/></param>
        public static IWebHost MigrateDatabase<TContext>(this IWebHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
                dbContext.Database.Migrate();
            }

            return host;
        }
    }
}
