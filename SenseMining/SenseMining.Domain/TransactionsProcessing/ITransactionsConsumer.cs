using System.Collections.Generic;
using System.Threading.Tasks;

namespace SenseMining.Domain.TransactionsProcessing
{
    public interface ITransactionsConsumer
    {
        Task ReceiveTransaction(List<string> transactionItems);
    }
}
