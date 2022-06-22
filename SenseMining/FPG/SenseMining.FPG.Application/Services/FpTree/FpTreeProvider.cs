using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SenseMining.FPG.Application.Abstractions;
using SenseMining.FPG.Application.Services.FpTree.Models;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Application.Services.FpTree;

public class FpTreeProvider : IFpTreeProvider //TODO repository
{
    private readonly IFpgDbContext _dbContext;
    private readonly CancellationToken _cancellationToken;
    private readonly IMapper _mapper;

    public FpTreeProvider(IFpgDbContext dbContext, CancellationTokenSource cancellationTokenSource, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _cancellationToken = cancellationTokenSource.Token;
    }

    public async Task<IEnumerable<Node>> GetActualFpTree()
    {
        return await _dbContext.FpTree.Include(a => a.Parent).Include(a => a.Subject).ToListAsync(_cancellationToken);
    }

    public async Task<FpTreeModel> GetFpTreeModel()
    {
        var tree = await _dbContext.FpTree.Include(a => a.Children).Include(a => a.Subject)
            .ToListAsync(_cancellationToken);
        var root = tree.Single(a => !a.ParentId.HasValue);

        return new FpTreeModel(DateTimeOffset.UtcNow, _mapper.Map<List<FpTreeNodeModel>>(root.Children));
    }
}