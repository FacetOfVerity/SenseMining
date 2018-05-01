using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SenseMining.Entities;

namespace SenseMining.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Node> FpTree { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionItem> TransactionItems { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }
        public DatabaseContext() : base()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TransactionItem>().HasKey(a => new {a.TransactionId, a.ProductId});
        }
    }
}
