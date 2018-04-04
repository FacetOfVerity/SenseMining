using System.ComponentModel.DataAnnotations;

namespace SenseMining.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        public Product() { }

        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
