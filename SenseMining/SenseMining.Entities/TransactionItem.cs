using System.ComponentModel.DataAnnotations.Schema;

namespace SenseMining.Entities
{
    public class TransactionItem
    {
        [ForeignKey(nameof(Transaction))]
        public int TransactionId { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Transaction Transaction { get; set; }

        public Product Product { get; set; }
    }
}
