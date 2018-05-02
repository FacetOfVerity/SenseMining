using System;
using System.Threading.Tasks;
using SenseMining.Domain.Services;
using SenseMining.Worker.Abstractions;

namespace SenseMining.Worker
{
    public class FpTreeRelevanceJob : IJob
    {
        private readonly IFpTreeService _fpTreeService;

        public FpTreeRelevanceJob(IFpTreeService fpTreeService)
        {
            _fpTreeService = fpTreeService;
        }

        public TimeSpan Interval => TimeSpan.FromSeconds(15);

        public async Task Execute()
        {
            await _fpTreeService.UpdateTree();
        }
    }
}
