using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Infrastructure.Database.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(a => a.Id);
    }
}