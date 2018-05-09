using System.Collections.Generic;
using SenseMining.Entities;

namespace SenseMining.Domain.Services.FpTree.Models
{
    public class FrequentItemsetModel
    {
        public int Support { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
