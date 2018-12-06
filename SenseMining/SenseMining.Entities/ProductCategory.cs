using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SenseMining.Entities
{
    /// <summary>
    /// Категория продукта
    /// </summary>
    public class ProductCategory
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        /// <summary/>
        public ProductCategory()
        {
            Id = Guid.NewGuid();
        }

        /// <summary/>
        public ProductCategory(string name) : this()
        {
            Name = name;
        }
    }
}
