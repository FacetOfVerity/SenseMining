using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Infrastructure.Database.Configurations;

public class TransactionItemConfiguration : IEntityTypeConfiguration<TransactionItem>
{
    public void Configure(EntityTypeBuilder<TransactionItem> builder)
    {
        builder.HasKey(a => new {a.TransactionId, a.SubjectId});
        
        builder
            .HasOne(a => a.Transaction)
            .WithMany()
            .HasForeignKey(a => a.TransactionId);
        
        builder
            .HasOne(a => a.Subject)
            .WithMany()
            .HasForeignKey(a => a.SubjectId);
    }
}