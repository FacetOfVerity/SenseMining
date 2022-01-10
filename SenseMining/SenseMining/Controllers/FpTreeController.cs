using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SenseMining.Domain.Services.FpTree;
using SenseMining.Domain.Services.FpTree.Models;

namespace SenseMining.API.Controllers
{
    /// <summary>
    /// API для работы с FP-деревом
    /// </summary>
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

        /// <summary>
        /// Получение модели FP-дерева (Json)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<FpTreeModel> GetFpTree()
        {
            return await _fpTreeProvider.GetFpTreeModel();
        }

        /// <summary>
        /// Запуск обновления FP-дерева
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateTree()
        {
            await _fpTreeService.UpdateTree();
        }

        /// <summary>
        /// Получение частых наборов для заданного значения минимальной поддержки
        /// </summary>
        /// <param name="minSupport">Поддержка</param>
        /// <returns></returns>
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

        /// <summary>
        /// Получение ассоциативных правил для заданного значения надежности
        /// </summary>
        /// <param name="minSupport"></param>
        /// <param name="minconf"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AssociationRules")]
        public async Task<object> GetAssociationRules([FromQuery] int minSupport, [FromQuery]int minconf)
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
