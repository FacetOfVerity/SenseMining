using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SenseMining.Database;
using SenseMining.Domain.Services.FpTree.Models;
using SenseMining.Domain.Utils;
using SenseMining.Entities;
using SenseMining.Entities.FpTree;

namespace SenseMining.Domain.Services.FpTree
{
    public class FpTreeService : IFpTreeService
    {
        private readonly IFpTreeProvider _fpTreeProvider;
        private readonly ITransactionsService _transactionsService;
        private readonly IProductsService _productsService;
        private readonly DatabaseContext _dbContext;
        private readonly CancellationToken _cancellationToken;
        

        public FpTreeService(IFpTreeProvider fpTreeProvider, ITransactionsService transactionsService,
            IProductsService productsService, DatabaseContext dbContext, CancellationTokenSource cancellationTokenSource)
        {
            _fpTreeProvider = fpTreeProvider;
            _transactionsService = transactionsService;
            _productsService = productsService;
            _dbContext = dbContext;
            _cancellationToken = cancellationTokenSource.Token;
        }

        public async Task UpdateTree()
        {
            var lastUpdate = await _dbContext.UpdateHistory.OrderByDescending(a => a.CreationTime).FirstOrDefaultAsync(_cancellationToken);
            var order = await _productsService.GetOrderedProducts();

            if (lastUpdate == null)
            {
                await BuildNewTree(order);
            }
            else
            {
                var transactions = await _transactionsService.GetLastTransactions(lastUpdate.CreationTime);
                //var root = await _dbContext.FpTree.Include(a => a.Children).SingleAsync(a => a.Id == lastUpdate.RootId, _cancellationToken);
                var tree = await _fpTreeProvider.GetActualFpTree();
                var root = tree.Single(a => a.Id == lastUpdate.RootId);

                UpdateTree(transactions, order, root);
                _dbContext.UpdateHistory.Add(new FpTreeUpdateInfo(root.Id, DateTimeOffset.UtcNow));
            }

            await _dbContext.SaveChangesAsync(_cancellationToken);
        }

        private async Task BuildNewTree(List<Product> order)
        {
            var transactions = await _transactionsService.GetLastTransactions(DateTimeOffset.MinValue);
            var root = new Node();

            UpdateTree(transactions, order, root);
            _dbContext.FpTree.Add(root);
            _dbContext.UpdateHistory.Add(new FpTreeUpdateInfo(root.Id, DateTimeOffset.UtcNow));
        }

        public async Task<List<FrequentItemsetModel>> ExtractFrequentItemsets(int minSupport)
        {
            var products = await _productsService.GetOrderedProducts();
            var tree = await _fpTreeProvider.GetActualFpTree();

            var result = new List<FrequentItemsetModel>();

            foreach (var product in products)
            {
                //Условный базис
                var conditionalBase = tree.Where(a => a.ProductId == product.Id)
                    .Select(a => a.PathToRoot.Select(n =>
                        new
                        {
                            ProductId = n.Node.ProductId.Value,
                            n.Node.Product,
                            n.Node.Support,
                            ConditionalSupport = a.Support,
                            Item = n
                        }));

                //Суммарная поддержка каждого элемента в условном базисе
                var totalSups = conditionalBase.SelectMany(a => a)
                    .GroupBy(a => a.ProductId).ToDictionary(a => a.Key, a => a.Sum(p => p.ConditionalSupport));

                foreach (var branch in conditionalBase)
                {
                    foreach (var item in branch)
                    {
                        var support = totalSups[item.ProductId];

                        //Фильтрация редких элементов
                        if (support < minSupport)
                            continue;

                        var set = CollectFrequentItemsets(item.Item, support, totalSups);

                        //Фильтрация повторов
                        if (result.Any(a => a.Products.Intersect(set).Count() == set.Count()))
                            continue;

                        result.Add(new FrequentItemsetModel
                        {
                            Support = support,
                            Products = set
                        });
                    }
                }
            }

            return result;
        }

        private IEnumerable<Product> CollectFrequentItemsets(ConditionalTreeItem root, int support, Dictionary<Guid, int> totalSups)
        {
            var node = root;
            do
            {
                if (totalSups.TryGetValue(node.Node.ProductId.Value, out int s) && s < support) continue;

                yield return node.Node.Product;
            } while ((node = node.Next) != null);
        }

        private void UpdateTree(List<Transaction> transactions, List<Product> order, Node root)
        {
            var comparer = new ProductsComparer(order);

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
        }
    }
}
