namespace SenseMining.FPG.Domain.FpTreeAggregate;

public class Node
{
    public Guid Id { get; set; }
        
    public Guid? ParentId { get; set; }
        
    public Guid? SubjectId { get; set; }

    public int Support { get; set; }

    public Subject Subject { get; set; }

    public Node Parent { get; set; }

    public ICollection<Node> Children { get; set; }

    public Node()
    {
        Id = Guid.NewGuid();
        Children = new List<Node>();  
    }

    public Node(Guid subjectId, Guid? parentId, int support) : this()
    {
        SubjectId = subjectId;
        ParentId = parentId;
        Support = support;
    }

    public override string ToString()
    {
        return Subject?.ToString() ?? "no product";
    }
        
    public bool IsRoot => !ParentId.HasValue;
        
    public IEnumerable<ConditionalTreeItem> PathToRoot
    {
        get
        {
            var node = this;
            var next = new ConditionalTreeItem(node);

            while (!(node = node.Parent).IsRoot)
            {
                var result = new ConditionalTreeItem(node, next);
                
                yield return result;

                next = result;
            }
        }
    }
}