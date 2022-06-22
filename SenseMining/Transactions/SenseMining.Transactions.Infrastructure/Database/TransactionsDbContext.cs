using Microsoft.EntityFrameworkCore;
using SenseMining.Transactions.Application.Abstractions;
using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Infrastructure.Database;

public class TransactionsDbContext : DbContext, ITransactionsDbContext
{
    public DbSet<Subject> Subjects { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<TransactionItem> TransactionItems { get; set; }
        

    public TransactionsDbContext(DbContextOptions options) : base(options)
    {
            
    }
}