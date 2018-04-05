using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SenseMining.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public ICollection<TransactionItem> Items { get; set; }

        public Transaction()
        {
            
        }

        public Transaction(int tid)
        {
            Id = tid;
        }
    }
}
