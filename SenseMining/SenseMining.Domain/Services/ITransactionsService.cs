using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SenseMining.Entities;

namespace SenseMining.Domain.Services
{
    public interface ITransactionsService
    {
        Task InsertTransaction(List<string> transactionItems);

        Task<List<Transaction>> GetLastTransactions(DateTimeOffset dateFrom);
    }
}
