using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SenseMining.Database;
using SenseMining.Entities;

namespace SenseMining.Domain.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly DatabaseContext _dbContext;
        private readonly CancellationToken _cancellationToken;
        private readonly IProductsService _productsService;

        public TransactionsService(DatabaseContext dbContext, CancellationTokenSource cancellationTokenSource,
            IProductsService productsService)
        {
            _dbContext = dbContext;
            _productsService = productsService;
            _cancellationToken = cancellationTokenSource.Token;
        }

        public async Task InsertTransaction(IEnumerable<string> transactionItems, bool saveImmediately)
        {
            var existing = await _productsService.DefineTransactionProducts(transactionItems);
            await _productsService.IncrementFrequencies(existing.Select(a => a.Id), false);

            var newProducts = await RegisterNewProducts(transactionItems, existing);
            newProducts.AddRange(existing);

            var transaction = new Transaction();
            var items = newProducts.Select(a => new TransactionItem(transaction.Id, a.Id));
            foreach (var transactionItem in items)
            {
                _dbContext.TransactionItems.Add(transactionItem);
            }

            _dbContext.Transactions.Add(transaction);

            if (saveImmediately)
            {
                await _dbContext.SaveChangesAsync(_cancellationToken);
            }
        }

        private async Task<List<Product>> RegisterNewProducts(IEnumerable<string> all, List<Product> existing)
        {
            var newProducts = all.Except(existing.Select(a => a.Name));

            return await _productsService.InsertProducts(newProducts, false);
        }
    }
}
