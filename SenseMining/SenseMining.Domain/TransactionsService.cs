using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SenseMining.Database;
using SenseMining.Entities;

namespace SenseMining.Domain
{
    public class TransactionsService : ITransactionsService
    {
        private readonly DatabaseContext _dbContext;
        private readonly CancellationToken _cancellationToken;
        private readonly IFrequenciesService _frequenciesService;
        private readonly IProductsService _productsService;

        public TransactionsService(DatabaseContext dbContext, CancellationTokenSource cancellationTokenSource,
            IFrequenciesService frequenciesService, IProductsService productsService)
        {
            _dbContext = dbContext;
            _frequenciesService = frequenciesService;
            _productsService = productsService;
            _cancellationToken = cancellationTokenSource.Token;
        }

        public async Task PostTransaction(List<string> products)
        {
            var productsList = await _productsService.DefineTransactionProducts(products, false);
            var transaction = new Transaction();
            transaction.Items = productsList.Select(a => new TransactionItem
            {
                Product = a,
                Transaction = transaction
            }).ToList();

            _dbContext.Transactions.Add(transaction);

            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
    }
}
