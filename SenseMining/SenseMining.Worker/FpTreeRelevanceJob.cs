using System;
using System.Threading.Tasks;
using SenseMining.Domain.Services.FpTree;
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

        public TimeSpan Interval => TimeSpan.FromHours(24);

        public async Task Execute()
        {
            await _fpTreeService.UpdateTree();
        }
    }
}
