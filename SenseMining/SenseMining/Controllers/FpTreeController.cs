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
    }
}
