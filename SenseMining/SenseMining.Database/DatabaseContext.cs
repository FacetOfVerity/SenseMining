using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SenseMining.Entities;
using SenseMining.Entities.FpTree;

namespace SenseMining.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Node> FpTree { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionItem> TransactionItems { get; set; }

        public DbSet<FpTreeUpdateInfo> UpdateHistory { get; set; }

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
            //modelBuilder.Entity<Node>().HasMany(a => a.Children).WithOne(a => a.Parent).HasForeignKey(a => a.ParentId);
        }
    }
}
