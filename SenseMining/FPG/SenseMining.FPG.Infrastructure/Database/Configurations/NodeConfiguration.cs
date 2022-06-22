using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Infrastructure.Database.Configurations;

public class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.HasKey(a => a.Id);

        builder
            .HasOne(a => a.Parent)
            .WithMany(a => a.Children)
            .HasForeignKey(a => a.ParentId);

        builder
            .Ignore(a => a.IsRoot)
            .Ignore(a => a.PathToRoot);
    }
}