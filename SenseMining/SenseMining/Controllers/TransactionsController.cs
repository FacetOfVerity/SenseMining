using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SenseMining.Domain;

namespace SenseMining.API.Controllers
{
    [Route("Transactions")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpPost]
        public async Task PostTransaction(List<string> products)
        {
            await _transactionsService.PostTransaction(products);
        }
    }
}
