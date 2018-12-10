using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SenseMining.Database;
using SenseMining.Domain.Services;

namespace SenseMining.Domain.TransactionsProcessing
{
    public class TransactionsProcessor : ITransactionsProcessor
    {
        private readonly DatabaseContext _context;
        private readonly CancellationToken _cancellationToken;
        private readonly ITransactionsService _transactionsService;

        public TransactionsProcessor(DatabaseContext context, CancellationTokenSource cancellationTokenSource,
            ITransactionsService transactionsService)
        {
            _context = context;
            _transactionsService = transactionsService;
            _cancellationToken = cancellationTokenSource.Token;
        }

        public async Task ReceiveTransaction(IEnumerable<string> transactionItems)
        {
            await _transactionsService.InsertTransaction(transactionItems);
        }
    }
}
