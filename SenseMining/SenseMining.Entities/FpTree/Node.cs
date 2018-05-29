using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenseMining.Entities.FpTree
{
    public class Node
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
            return Product?.ToString() ?? "no product";
        }

        [NotMapped]
        public bool IsRoot => !ParentId.HasValue;

        [NotMapped]
        public IEnumerable<ConditionalTreeItem> PathToRoot
        {
            get
            {
                var node = this;
                var next = new ConditionalTreeItem(node);

                while (!(node = node.Parent).IsRoot)
                {
                    var result = new ConditionalTreeItem
                    {
                        Node = node,
                        Next = next
                    };
                    yield return result;

                    next = result;
                }
            }
        }
    }
}
