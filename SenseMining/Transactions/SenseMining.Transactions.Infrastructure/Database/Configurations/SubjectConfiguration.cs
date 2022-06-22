using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Infrastructure.Database.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.Category).WithMany().HasForeignKey(a => a.CategoryId);
        
        builder
            .Property(a => a.Name)
            .HasMaxLength(100);
    }
}