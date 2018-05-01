using System.Collections.Generic;
using System.Threading.Tasks;
using SenseMining.Domain.Services;

namespace SenseMining.Domain.TransactionsProcessing
{
    public class TransactionsConsumer : ITransactionsConsumer
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsConsumer(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        public async Task ReceiveTransaction(IEnumerable<string> transactionItems)
        {
            await _transactionsService.InsertTransaction(transactionItems, false);
        }
    }
}
