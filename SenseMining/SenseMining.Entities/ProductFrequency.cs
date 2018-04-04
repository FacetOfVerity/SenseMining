using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenseMining.Entities
{
    public class ProductFrequency
    {
        [Key]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public long Value { get; set; }

        public Product Product { get; set; }

        public ProductFrequency()
        {
            
        }

        public ProductFrequency(int productId, long value)
        {
            ProductId = productId;
            Value = value;
        }
    }
}
