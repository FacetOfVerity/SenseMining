using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Infrastructure.Database.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(a => a.Id);
    }
}