using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SenseMining.Database;
using SenseMining.Domain.Services.FpTree.Models;
using SenseMining.Domain.Utils;
using SenseMining.Entities;

namespace SenseMining.Domain.Services.FpTree
{
    public class FpTreeService : IFpTreeService
    {
        private readonly ILogger<FpTreeService> _logger;
        private readonly ITransactionsService _transactionsService;
        private readonly IProductsService _productsService;
        private readonly DatabaseContext _dbContext;
        private readonly CancellationToken _cancellationToken;
        private readonly IMapper _mapper;

        public FpTreeService(ILogger<FpTreeService> logger, ITransactionsService transactionsService,
            IProductsService productsService, DatabaseContext dbContext,
            CancellationTokenSource cancellationTokenSource, IMapper mapper)
        {
            _logger = logger;
            _transactionsService = transactionsService;
            _productsService = productsService;
            _dbContext = dbContext;
            _mapper = mapper;
            _cancellationToken = cancellationTokenSource.Token;
        }

        public async Task<FpTreeModel> GetTreeFromDatabase()
        {
            var tree = await _dbContext.FpTree.Include(a => a.Children).Include(a => a.Product).ToListAsync(_cancellationToken);
            var root = tree.Single(a => !a.ParentId.HasValue);

            return new FpTreeModel(DateTimeOffset.UtcNow, _mapper.Map<List<FpTreeNodeModel>>(root.Children));
        }

        public async Task UpdateTree()
        {
            await ClearTree();

            var order = await _productsService.GetOrderedProducts();
            var comparer = new ProductsComparer(order);

            var transactions = await _transactionsService.GetLastTransactions(DateTimeOffset.Now.AddDays(-1));
            var root = BuildTree(transactions, comparer);

            _dbContext.FpTree.Add(root);

            await _dbContext.SaveChangesAsync(_cancellationToken);
        }

        public async Task<List<FrequentItemsetModel>> GetFrequentItemsets(int minSupport)
        {
            var products = await _productsService.GetOrderedProducts();
            var tree = await _dbContext.FpTree.Include(a => a.Parent).ToListAsync(_cancellationToken);

            var result = new List<FrequentItemsetModel>();

            foreach (var product in products)
            {
                var conditionalBase = tree.Where(a => a.ProductId == product.Id)
                    .Select(a => a.PathToRoot.Select(n => new
                    {
                        n.ProductId,
                        a.Support,
                        n.Product
                    }));

                var pairs = conditionalBase.SelectMany(a => a)
                    .GroupBy(a => a.ProductId)
                    .Select(a => new
                    {
                        ProductId = a.Key,
                        a.First().Product,
                        Support = a.Sum(p => p.Support)
                    })
                    .Where(a => a.Support >= minSupport).Select(a => new FrequentItemsetModel
                    {
                        Support = a.Support,
                        Products = new List<Product>
                        {
                            product,
                            a.Product
                        }
                    });

                result.AddRange(pairs);
            }

            return result;
        }

        private Node BuildTree(List<Transaction> transactions, ProductsComparer comparer)
        {
            var root = new Node();

            foreach (var transaction in transactions)
            {
                var orderedItems = transaction.Items.OrderBy(a => a.ProductId, comparer);

                var tempRoot = root;
                Node tempNode;
                foreach (var transactionItem in orderedItems)
                {
                    var aNode = new Node(transactionItem.ProductId, tempRoot.Id, 1);

                    if ((tempNode = tempRoot.Children.FirstOrDefault(c => c.ProductId == aNode.ProductId)) != null)
                    {
                        tempNode.Support++;
                        tempRoot = tempNode;
                    }
                    else
                    {
                        tempRoot.Children.Add(aNode);
                        tempRoot = aNode;
                    }
                }
            }

            return root;
        }

        private async Task ClearTree()
        {
            var oldTree = await _dbContext.FpTree.Include(a => a.Children).ToListAsync(_cancellationToken);
            _dbContext.FpTree.RemoveRange(oldTree);
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
    }
}
