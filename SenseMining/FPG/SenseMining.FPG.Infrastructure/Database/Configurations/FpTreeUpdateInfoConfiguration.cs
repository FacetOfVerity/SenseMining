using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Infrastructure.Database.Configurations;

public class FpTreeUpdateInfoConfiguration : IEntityTypeConfiguration<FpTreeUpdateInfo>
{
    public void Configure(EntityTypeBuilder<FpTreeUpdateInfo> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder
            .HasOne(a => a.Root)
            .WithMany()
            .HasForeignKey(a => a.RootId);
    }
}