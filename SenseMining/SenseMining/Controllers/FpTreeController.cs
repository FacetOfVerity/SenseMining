using System.Collections.Generic;
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

        public FpTreeController(IFpTreeService fpTreeService)
        {
            _fpTreeService = fpTreeService;
        }

        [HttpGet]
        public async Task<FpTreeModel> GetFpTree()
        {
            return await _fpTreeService.GetTreeFromDatabase();
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
            var result = await _fpTreeService.GetFrequentItemsets(minSupport);
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
