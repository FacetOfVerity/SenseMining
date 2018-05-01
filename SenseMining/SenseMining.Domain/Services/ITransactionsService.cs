using System.Collections.Generic;
using System.Threading.Tasks;

namespace SenseMining.Domain.Services
{
    public interface ITransactionsService
    {
        Task InsertTransaction(List<string> transactionItems);
    }
}
