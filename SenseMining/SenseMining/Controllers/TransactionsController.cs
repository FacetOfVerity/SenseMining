using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SenseMining.Domain.TransactionsProcessing;

namespace SenseMining.API.Controllers
{
    [Route("Transactions")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionsConsumer _transactionsConsumer;

        public TransactionsController(ITransactionsConsumer transactionsConsumer)
        {
            _transactionsConsumer = transactionsConsumer;
        }


        [HttpPost]
        public async Task PostTransaction(List<string> products)
        {
            await _transactionsConsumer.ReceiveTransaction(products);
        }
    }
}
