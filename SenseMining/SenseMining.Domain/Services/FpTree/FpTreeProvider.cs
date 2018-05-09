using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SenseMining.Database;
using SenseMining.Domain.Services.FpTree.Models;
using SenseMining.Entities;

namespace SenseMining.Domain.Services.FpTree
{
    public class FpTreeProvider : IFpTreeProvider
    {
        private readonly DatabaseContext _dbContext;
        private readonly CancellationToken _cancellationToken;
        private readonly IMapper _mapper;

        public FpTreeProvider(DatabaseContext dbContext, CancellationTokenSource cancellationTokenSource, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cancellationToken = cancellationTokenSource.Token;
        }

        public async Task<List<Node>> GetActualFpTree()
        {
            return await _dbContext.FpTree.Include(a => a.Parent).Include(a => a.Product).ToListAsync(_cancellationToken);
        }

        public async Task<FpTreeModel> GetFpTreeModel()
        {
            var tree = await _dbContext.FpTree.Include(a => a.Children).Include(a => a.Product).ToListAsync(_cancellationToken);
            var root = tree.Single(a => !a.ParentId.HasValue);

            return new FpTreeModel(DateTimeOffset.UtcNow, _mapper.Map<List<FpTreeNodeModel>>(root.Children));
        }
    }
}
