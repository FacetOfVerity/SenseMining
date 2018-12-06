using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenseMining.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Category))]
        public Guid? CategoryId { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        public int Support { get; set; }

        public virtual ProductCategory Category { get; set; }

        public Product() { }

        public Product(string name)
        {
            Id = Guid.NewGuid();
            Support = 1;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
