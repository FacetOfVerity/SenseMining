using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SenseMining.Domain.Services
{
    public class FpTreeService : IFpTreeService
    {
        private readonly ILogger<FpTreeService> _logger;

        public FpTreeService(ILogger<FpTreeService> logger)
        {
            _logger = logger;
        }

        public async Task UpdateTree()
        {
            _logger.LogInformation("Дерево обновлено");
            await Task.CompletedTask;
        }
    }
}
