using Common.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Application.Abstractions;

public interface ITransactionsDbContext : IContext
{
    public DbSet<Subject> Subjects { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<TransactionItem> TransactionItems { get; set; }
}