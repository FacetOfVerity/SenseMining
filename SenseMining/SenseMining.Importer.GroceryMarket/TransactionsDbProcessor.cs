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
        private readonly ImporterOptions _options;

        public TransactionsDbProcessor(CancellationTokenSource cancellationTokenSource, ImporterOptions options)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _options = options;
        }

        public async Task Start()
        {
            try
            {
                using (var reader = new ChoCSVReader<SalesRowModel>(Path.Combine(_options.ResourcesPath,"sales.csv")).WithFirstLineHeader())
                {
                    var groups = reader.Take(1000000).GroupBy(a => a.TransactionNumber).Where(a => a.Count() > 1).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
