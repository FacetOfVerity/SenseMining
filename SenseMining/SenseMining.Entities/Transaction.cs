using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenseMining.Entities
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
