namespace SenseMining.FPG.Domain.FpTreeAggregate;

public record ConditionalTreeItem(Node Node, ConditionalTreeItem Next)
{
    public ConditionalTreeItem(Node node) : this(node, null)
    {
    }
}