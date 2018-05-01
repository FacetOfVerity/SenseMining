using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenseMining.Entities
{
    public class Node
    {
        [Key]
        [ForeignKey(nameof(Product))]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; set; }

        public int Score { get; set; }

        public Product Product { get; set; }

        public Node Parent { get; set; }

        public ICollection<Node> Children { get; set; }

        public Node()
        {
            
        }

        public Node(Guid id, Guid? parentId, int score)
        {
            Id = id;
            ParentId = parentId;
            Score = score;
        }
    }
}
