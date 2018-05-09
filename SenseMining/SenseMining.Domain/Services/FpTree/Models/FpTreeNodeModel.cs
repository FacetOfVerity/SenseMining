using System.Collections.Generic;

namespace SenseMining.Domain.Services.FpTree.Models
{
    public class FpTreeNodeModel
    {
        public int Support { get; set; }

        public string Product { get; set; }

        public List<FpTreeNodeModel> Children { get; set; }
    }
}
