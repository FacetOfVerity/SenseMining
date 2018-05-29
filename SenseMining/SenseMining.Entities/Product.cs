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

        public int Support { get; set; }

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
