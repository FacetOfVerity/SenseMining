using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenseMining.Entities
{
    public class Node
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid? ProductId { get; set; }

        public int Score { get; set; }

        public Product Product { get; set; }

        public Node Parent { get; set; }

        public ICollection<Node> Children { get; set; }

        public Node()
        {
            Id = Guid.NewGuid();
            Children = new List<Node>();  
        }

        public Node(Guid productId, Guid? parentId, int score) : this()
        {
            ProductId = productId;
            ParentId = parentId;
            Score = score;
        }

        public override string ToString()
        {
            return $"{Id} count={Score}";
        }
    }
}
