using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenseMining.Entities
{
    public class TransactionItem
    {
        [ForeignKey(nameof(Transaction))]
        public Guid TransactionId { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public Transaction Transaction { get; set; }

        public Product Product { get; set; }

        public TransactionItem()
        {
            
        }

        public TransactionItem(Guid transactionId, Guid productId)
        {
            TransactionId = transactionId;
            ProductId = productId;
        }
    }
}
