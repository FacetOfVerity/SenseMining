using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SenseMining.Database;

namespace SenseMining.Domain
{
    public class FrequenciesService : IFrequenciesService
    {
        private readonly DatabaseContext _dbContext;
        private readonly CancellationToken _cancellationToken;

        public FrequenciesService(DatabaseContext dbContext, CancellationTokenSource cancellationTokenSource)
        {
            _dbContext = dbContext;
            _cancellationToken = cancellationTokenSource.Token;
        }

        public async Task IncrementFrequencies(List<int> productsIds, bool saveImmediately)
        {
            var products = _dbContext.Products.Where(a => productsIds.Contains(a.Id));
            foreach (var product in products)
            {
                product.Id++;
            }

            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
    }
}
