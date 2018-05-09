namespace SenseMining.Entities.FpTree
{
    public class ConditionalTreeItem
    {
        public Node Node { get; set; }
        public ConditionalTreeItem Next { get; set; }

        public ConditionalTreeItem(Node node)
        {
            Node = node;
        }

        public ConditionalTreeItem()
        {
            
        }

        public override string ToString()
        {
            return $"node: {Node.Product.Name}, next: {Next?.Node?.Product?.Name}";
        }
    }
}
