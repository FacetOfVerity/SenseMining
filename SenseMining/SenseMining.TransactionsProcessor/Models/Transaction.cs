using System.Collections.Generic;

namespace SenseMining.TransactionsProcessor.Models
{
    /// <summary>
    /// Транзакция
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Набор элементов транзации
        /// </summary>
        public IEnumerable<string> Items { get; set; }
    }
}
