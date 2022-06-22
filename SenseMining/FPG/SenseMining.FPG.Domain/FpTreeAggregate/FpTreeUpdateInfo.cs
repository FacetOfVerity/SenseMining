namespace SenseMining.FPG.Domain.FpTreeAggregate;

public class FpTreeUpdateInfo
{
    public int Id { get; set; }
        
    public Guid RootId { get; set; }

    public DateTimeOffset CreationTime { get; set; }

    public Node Root { get; set; }

    public FpTreeUpdateInfo()
    {
            
    }

    public FpTreeUpdateInfo(Guid rootId, DateTimeOffset creationTime)
    {
        RootId = rootId;
        CreationTime = creationTime;
    }
}