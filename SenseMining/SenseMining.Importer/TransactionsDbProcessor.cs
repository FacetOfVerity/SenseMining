using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using SenseMining.Domain.TransactionsProcessing;
using SenseMining.Importer.Models;

namespace SenseMining.Importer
{
    public class TransactionsDbProcessor
    {
        private readonly IMongoCollection<StoredTransactionModel> _dataSource;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ITransactionsConsumer _transactionsConsumer;

        public TransactionsDbProcessor(ConnectionOptions options, IMongoDatabase database, CancellationTokenSource cancellationTokenSource, ITransactionsConsumer transactionsConsumer)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _transactionsConsumer = transactionsConsumer;
            _dataSource = database.GetCollection<StoredTransactionModel>(options.FiscalDataCollectionName);
        }

        public async Task Start()
        {
            var filter = Builders<StoredTransactionModel>.Filter.Empty;
            var count = await _dataSource.CountDocumentsAsync(filter, cancellationToken: _cancellationTokenSource.Token);

            var step = 100;
            for (int i = 0; i < count; i+= step)
            {
                var set = await _dataSource.Find(filter).Skip(i).Limit(step).ToListAsync(_cancellationTokenSource.Token);
                
                foreach (var transaction in set)
                {
                    await _transactionsConsumer.ReceiveTransaction(transaction.Document.Order.Select(a => a.Name).ToList());
                }

                Console.WriteLine($"Еще {step} готовы");
            }
        }
    }
}
