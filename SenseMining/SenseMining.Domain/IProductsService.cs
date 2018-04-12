using System.Collections.Generic;
using System.Threading.Tasks;
using SenseMining.Entities;

namespace SenseMining.Domain
{
    public interface IProductsService
    {
        Task<List<Product>> DefineTransactionProducts(List<string> products, bool saveImmediately);
    }
}
