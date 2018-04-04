using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SenseMining.Entities;

namespace SenseMining.Database
{
    public class DatabaseContext : DbContext
    {
        public ICollection<Product> Products { get; set; }

        public ICollection<Node> FpTree { get; set; }

        public ICollection<ProductFrequency> Frequencies { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public ICollection<TransactionItem> TransactionItems { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TransactionItem>().HasKey(a => new {a.TransactionId, a.ProductId});
        }
    }
}
