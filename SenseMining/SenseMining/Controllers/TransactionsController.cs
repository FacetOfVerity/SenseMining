using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SenseMining.Domain.TransactionsProcessing;

namespace SenseMining.API.Controllers
{
    [Route("Transactions")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionsProcessor _transactionsProcessor;

        public TransactionsController(ITransactionsProcessor transactionsProcessor)
        {
            _transactionsProcessor = transactionsProcessor;
        }


        [HttpPost]
        public async Task PostTransaction(List<string> products)
        {
            await _transactionsProcessor.ReceiveTransaction(products);
        }
    }
}
