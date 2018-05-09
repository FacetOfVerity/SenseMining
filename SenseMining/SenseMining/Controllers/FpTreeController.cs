using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SenseMining.Domain.Services.FpTree;
using SenseMining.Domain.Services.FpTree.Models;

namespace SenseMining.API.Controllers
{
    [Route("FpTree")]
    public class FpTreeController : Controller
    {
        private readonly IFpTreeService _fpTreeService;
        private readonly IFpTreeProvider _fpTreeProvider;

        public FpTreeController(IFpTreeService fpTreeService, IFpTreeProvider fpTreeProvider)
        {
            _fpTreeService = fpTreeService;
            _fpTreeProvider = fpTreeProvider;
        }

        [HttpGet]
        public async Task<FpTreeModel> GetFpTree()
        {
            return await _fpTreeProvider.GetFpTreeModel();
        }

        [HttpPut]
        public async Task UpdateTree()
        {
            await _fpTreeService.UpdateTree();
        }

        [HttpGet]
        [Route("FrequentItemsets")]
        public async Task<object> GetFrequentItemsets([FromQuery] int minSupport)
        {
            var result = await _fpTreeService.ExtractFrequentItemsets(minSupport);
            return new
            {
                result.Count,
                Data = result.Select(a => new
                {
                    a.Support,
                    Products = a.Products.Select(p => p.Name)
                })
            };
        }
    }
}
