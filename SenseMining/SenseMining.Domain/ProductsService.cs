using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SenseMining.Entities;

namespace SenseMining.Domain
{
    public class ProductsService : IProductsService
    {
        public Task<List<Product>> DefineTransactionProducts(List<string> products, bool saveImmediately)
        {
            throw new NotImplementedException();
        }
    }
}
