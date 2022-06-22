using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Infrastructure.Database.Configurations;

public class SubjectCategoryConfiguration : IEntityTypeConfiguration<SubjectCategory>
{
    public void Configure(EntityTypeBuilder<SubjectCategory> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder
            .Property(a => a.Name)
            .HasMaxLength(100);
    }
}