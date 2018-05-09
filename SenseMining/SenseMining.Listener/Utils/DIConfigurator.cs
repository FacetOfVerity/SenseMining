using System;
using Microsoft.Extensions.DependencyInjection;

namespace SenseMining.Listener.Utils
{
    public static class DiConfigurator
    {
        public static IServiceProvider ServiceProvider { private set; get; }

        public static void SetUp(Action<IServiceCollection> setUp)
        {
            var services = new ServiceCollection();
            setUp.Invoke(services);

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
