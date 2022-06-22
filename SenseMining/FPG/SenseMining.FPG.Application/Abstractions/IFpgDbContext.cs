using Common.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Application.Abstractions;

public interface IFpgDbContext : IContext
{
    public DbSet<Subject> Subjects { get; set; }

    public DbSet<Node> FpTree { get; set; }

    public DbSet<FpTreeUpdateInfo> UpdateHistory { get; set; }
}