namespace SenseMining.FPG.Application.Services.FpTree.Models;

public class FpTreeNodeModel
{
    public int Support { get; set; }

    public Guid SubjectId { get; set; }

    public List<FpTreeNodeModel> Children { get; set; }
}