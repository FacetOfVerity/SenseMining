using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SenseMining.Entities
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public ICollection<TransactionItem> Items { get; set; }
    }
}
