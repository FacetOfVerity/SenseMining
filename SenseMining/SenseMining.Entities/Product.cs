using System;
using System.ComponentModel.DataAnnotations;

namespace SenseMining.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        public long Frequency { get; set; }

        public Product() { }

        public Product(string name)
        {
            Id = Guid.NewGuid();
            Frequency = 1;
            Name = name;
        }
    }
}
