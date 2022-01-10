using System.Collections.Generic;
using System.Threading.Tasks;

namespace SenseMining.Domain.TransactionsProcessing
{
    public interface ITransactionsProcessor
    {
        Task ReceiveTransaction(IEnumerable<string> transactionItems);
    }
}
