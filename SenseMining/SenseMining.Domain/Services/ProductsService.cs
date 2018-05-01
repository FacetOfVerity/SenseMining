using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SenseMining.Database;
using SenseMining.Entities;

namespace SenseMining.Domain.Services
{
    public class ProductsService : IProductsService
    {
        private readonly DatabaseContext _dbContext;
        private readonly CancellationToken _cancellationToken;

        public ProductsService(DatabaseContext dbContext, CancellationTokenSource cancellationTokenSource)
        {
            _dbContext = dbContext;
            _cancellationToken = cancellationTokenSource.Token;
        }

        public async Task<List<Product>> DefineTransactionProducts(List<string> products)
        {
            return await _dbContext.Products.AsQueryable()
                .Where(a => products.Contains(a.Name))
                .ToListAsync(_cancellationToken);
        }

        public async Task<List<Product>> InsertProducts(List<string> products, bool saveImmediately)
        {
            var productsList = products.Select(a => new Product(a)).ToList();
            productsList.ForEach(a => _dbContext.Products.Add(a));

            if (saveImmediately)
            {
                await _dbContext.SaveChangesAsync(_cancellationToken);
            }

            return productsList;
        }

        public async Task IncrementFrequencies(IEnumerable<Guid> productsIds, bool saveImmediately)
        {
            var products = await _dbContext.Products.AsQueryable().Where(a => productsIds.Contains(a.Id))
                .ToListAsync(_cancellationToken);
            products.ForEach(a => a.Frequency++);

            if (saveImmediately)
            {
                await _dbContext.SaveChangesAsync(_cancellationToken);
            }
        }
    }
}
