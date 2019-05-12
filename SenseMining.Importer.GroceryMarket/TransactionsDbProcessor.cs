using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChoETL;
using SenseMining.Importer.GroceryMarket.Models;

namespace SenseMining.Importer.GroceryMarket
{
    public class TransactionsDbProcessor
    {

        private readonly CancellationTokenSource _cancellationTokenSource;

        public TransactionsDbProcessor(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
        }

        public async Task Start()
        {
            using (var reader = new ChoCSVReader<SalesRowModel>(Path.Combine("Resources","sales", "sales-000.csv")).WithFirstLineHeader())
            {
                var first10 = reader.Take(10);
                //var groups = reader.GroupBy(a => a.TransactionNumber).ToArray();
            }
        }
    }
}
