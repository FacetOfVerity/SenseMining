using System;
using System.Collections.Generic;
using SenseMining.Entities;

namespace SenseMining.Domain.Utils
{
    public class ProductsComparer : IComparer<Guid>
    {
        private readonly Dictionary<Guid, int> _order;

        public ProductsComparer(List<Product> order)
        {
            _order = new Dictionary<Guid, int>();

            for (int i = 0; i < order.Count; i++)
            {
                _order.Add(order[i].Id, i);
            }
        }

        public int Compare(Guid x, Guid y)
        {
            if (x == y)
            {
                return 0;
            }

            return _order[x] > _order[y] ? 1 : -1;
        }
    }
}
