using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SenseMining.Entities;

namespace SenseMining.Domain.Services
{
    public interface IProductsService
    {
        Task<List<Product>> DefineTransactionProducts(List<string> products);

        Task<List<Product>> InsertProducts(List<string> products, bool saveImmediately);

        Task IncrementFrequencies(IEnumerable<Guid> productsIds, bool saveImmediately);
    }
}
