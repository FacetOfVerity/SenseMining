using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SenseMining.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        public ICollection<TransactionItem> Items { get; set; }

        public Transaction()
        {
            Id = Guid.NewGuid();
        }
    }
}
