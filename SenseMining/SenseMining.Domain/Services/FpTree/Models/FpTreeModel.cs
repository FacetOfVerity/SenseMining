using System;
using System.Collections.Generic;

namespace SenseMining.Domain.Services.FpTree.Models
{
    public class FpTreeModel
    {
        public DateTimeOffset CreationTime { get; set; }

        public List<FpTreeNodeModel> Children { get; set; }

        public FpTreeModel()
        {
            
        }

        public FpTreeModel(DateTimeOffset creationTime, List<FpTreeNodeModel> children)
        {
            CreationTime = creationTime;
            Children = children;
        }
    }
}
