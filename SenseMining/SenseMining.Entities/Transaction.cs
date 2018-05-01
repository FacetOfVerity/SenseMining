using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SenseMining.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public ICollection<TransactionItem> Items { get; set; }

        public Transaction()
        {
            CreationTime = DateTimeOffset.UtcNow;
            Id = Guid.NewGuid();
        }
    }
}
