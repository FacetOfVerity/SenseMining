using System.Collections.Generic;
using SenseMining.Entities;

namespace SenseMining.Domain.Services.FpTree.Models
{
    public class FpTreeNodeModel
    {
        public int Score { get; set; }

        public Product Product { get; set; }

        public List<FpTreeNodeModel> Children { get; set; }
    }
}
