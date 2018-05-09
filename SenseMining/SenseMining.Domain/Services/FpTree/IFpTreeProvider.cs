using System.Collections.Generic;
using System.Threading.Tasks;
using SenseMining.Domain.Services.FpTree.Models;
using SenseMining.Entities;

namespace SenseMining.Domain.Services.FpTree
{
    public interface IFpTreeProvider
    {
        Task<List<Node>> GetActualFpTree();

        Task<FpTreeModel> GetFpTreeModel();
    }
}
