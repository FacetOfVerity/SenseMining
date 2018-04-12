using System.Collections.Generic;
using System.Threading.Tasks;

namespace SenseMining.Domain
{
    public interface ITransactionsService
    {
        Task PostTransaction(List<string> productsIds);
    }
}
