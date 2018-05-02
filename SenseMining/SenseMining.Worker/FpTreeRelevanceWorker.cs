using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SenseMining.Worker
{
    public class FpTreeRelevanceWorker : IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly JobExecutor<FpTreeRelevanceJob> _executor;
        private readonly ILogger<FpTreeRelevanceWorker> _logger;

        public FpTreeRelevanceWorker(IServiceScopeFactory scopeFactory)
        {
            _scope = scopeFactory.CreateScope();
            _logger = _scope.ServiceProvider.GetService<ILogger<FpTreeRelevanceWorker>>();
            _executor = new JobExecutor<FpTreeRelevanceJob>(scopeFactory,
                _scope.ServiceProvider.GetService<ILogger<JobExecutor<FpTreeRelevanceJob>>>());
        }

        public void Run()
        {
            _executor.Start();
            _logger.LogInformation("Актуализатор FP дерева запущен");
        }

        public void Dispose()
        {
            _executor.Stop();
            _scope.Dispose();
            _logger.LogInformation("Актуализатор FP дерева остановлен");
        }
    }
}
