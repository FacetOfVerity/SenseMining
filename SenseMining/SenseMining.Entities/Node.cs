using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenseMining.Entities
{
    public class Node : ICloneable
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid? ProductId { get; set; }

        public int Support { get; set; }

        public Product Product { get; set; }

        public Node Parent { get; set; }

        public ICollection<Node> Children { get; set; }

        public Node()
        {
            Id = Guid.NewGuid();
            Children = new List<Node>();  
        }

        public Node(Guid productId, Guid? parentId, int support) : this()
        {
            ProductId = productId;
            ParentId = parentId;
            Support = support;
        }

        public override string ToString()
        {
            return $"{Id} Support={Support}";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        [NotMapped]
        public bool IsRoot => !ParentId.HasValue;

        [NotMapped]
        public IEnumerable<Node> PathToRoot
        {
            get
            {
                var n = this;
                while (!(n = n.Parent).IsRoot) yield return n;
            }
        }
    }
}
