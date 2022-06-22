using SenseMining.FPG.Application.Services.FpTree.Models;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Application.Services.FpTree;

public interface IFpTreeProvider
{
    Task<IEnumerable<Node>> GetActualFpTree();

    Task<FpTreeModel> GetFpTreeModel();
}