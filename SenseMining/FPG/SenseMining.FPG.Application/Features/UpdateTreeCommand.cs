using MediatR;
using Microsoft.EntityFrameworkCore;
using SenseMining.FPG.Application.Abstractions;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Application.Features;

public class UpdateTreeCommand : IRequest
{
    #region Data

    

    #endregion

    #region Handler

    public class Handler : IRequestHandler<UpdateTreeCommand>
    {
        private readonly IFpgDbContext _dbContext;

        public Handler(IFpgDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateTreeCommand request, CancellationToken cancellationToken)
        {
            var lastUpdate = await _dbContext.UpdateHistory.OrderByDescending(a => a.CreationTime).FirstOrDefaultAsync(_cancellationToken);
            //Получаем элементы в порядке убывания поддержки
            var order = await _subjectsService.GetOrderedSubjects();

            if (lastUpdate == null) //FP-дерево не создано
            {
                await BuildNewTree(order);
            }
            else
            {
                var transactions = await _transactionsService.GetLastTransactions(lastUpdate.CreationTime);
                //var tree = await _fpTreeProvider.GetActualFpTree();
                var tree = await _dbContext.FpTree.ToListAsync();
                
                var root = tree.Single(a => a.Id == lastUpdate.RootId);

                PushTransactions(transactions, order, root);
                _dbContext.UpdateHistory.Add(new FpTreeUpdateInfo(root.Id, DateTimeOffset.UtcNow));
            }

            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
        
        /// <summary>
        /// Постороение нового дерева
        /// </summary>
        /// <param name="order">Порядок сортировки</param>
        private async Task BuildNewTree(List<Subject> order)
        {
            var transactions = await _transactionsService.GetLastTransactions(DateTimeOffset.MinValue);
            var root = new Node();

            PushTransactions(transactions, order, root);
            _dbContext.FpTree.Add(root);
            _dbContext.UpdateHistory.Add(new FpTreeUpdateInfo(root.Id, DateTimeOffset.UtcNow));
        }
        
        /// <summary>
        /// Вставка в дерево списка транзакций
        /// </summary>
        /// <param name="transactions">Транзакции</param>
        /// <param name="order">Порядок</param>
        /// <param name="root">Корневой узел дерева</param>
        private void PushTransactions(List<Transaction> transactions, List<Subject> order, Node root)
        {
            var comparer = new SubjectsComparer(order); //Компаратор элементов

            foreach (var transaction in transactions)
            {
                var orderedItems = transaction.Items.OrderBy(a => a.ProductId, comparer);

                var tempRoot = root;
                Node tempNode;
                foreach (var transactionItem in orderedItems)
                {
                    var aNode = new Node(transactionItem.ProductId, tempRoot.Id, 1);

                    if ((tempNode = tempRoot.Children.FirstOrDefault(c => c.SubjectId == aNode.SubjectId)) != null)
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

    #endregion
}