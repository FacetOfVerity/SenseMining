using System;
using System.Collections.Generic;
using System.Text;

namespace SenseMining.Importer.GroceryMarket.Models
{
    public class SalesRowModel
    {
        public string SalesPersonID { get; set; }

        public string ProductID { get; set; }

        public DateTime? SalesDate { get; set; }

        public string TransactionNumber { get; set; }
    }
}
