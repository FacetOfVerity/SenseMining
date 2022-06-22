using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Application.Services;

public interface ITransactionsService
{
    Task InsertTransaction(IEnumerable<string> transactionItems);

    Task<List<Transaction>> GetLastTransactions(DateTimeOffset dateFrom);
}