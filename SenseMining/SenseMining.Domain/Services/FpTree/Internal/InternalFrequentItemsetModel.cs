using System;
using SenseMining.Entities;

namespace SenseMining.Domain.Services.FpTree.Internal
{
    internal class InternalFrequentItemsetModel
    {
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public int Support { get; set; }

        public int ConditionalSupport { get; set; }
    }
}
