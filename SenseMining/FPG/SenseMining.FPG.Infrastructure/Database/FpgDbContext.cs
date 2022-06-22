using Microsoft.EntityFrameworkCore;
using SenseMining.FPG.Application.Abstractions;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Infrastructure.Database;

public class FpgDbContext : DbContext, IFpgDbContext
{
    public DbSet<Subject> Subjects { get; set; }

    public DbSet<Node> FpTree { get; set; }

    public DbSet<FpTreeUpdateInfo> UpdateHistory { get; set; }

    public FpgDbContext(DbContextOptions options) : base(options)
    {
            
    }
}