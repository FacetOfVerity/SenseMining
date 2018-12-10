using System;
using System.Collections.Generic;
using System.Text;

namespace SenseMining.Importer.GroceryMarket.Models
{
    public class SalesRowModel
    {
        public int CustomerID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public DateTime? SalesDate { get; set; }

        public string TransactionNumber { get; set; }
    }
}
