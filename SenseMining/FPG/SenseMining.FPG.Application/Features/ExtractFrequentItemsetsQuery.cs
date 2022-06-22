using MediatR;
using SenseMining.FPG.Application.Services.FpTree;
using SenseMining.FPG.Application.Services.FpTree.Models;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Application.Features;

/// <summary>
/// Извлечение частых наборов из актуального дерева
/// </summary>
/// <param name="minSupport">Минимальная поддержка</param>
public class ExtractFrequentItemsetsQuery : IRequest<IEnumerable<FrequentItemsetDto>>
{
    #region Data

    private readonly int _minSupport;

    public ExtractFrequentItemsetsQuery(int minSupport)
    {
        _minSupport = minSupport;
    }

    #endregion

    #region Handler
    
    public class Handler : IRequestHandler<ExtractFrequentItemsetsQuery, IEnumerable<FrequentItemsetDto>>
    {
        private readonly IFpTreeProvider _fpTreeProvider;

        public Handler(IFpTreeProvider fpTreeProvider)
        {
            _fpTreeProvider = fpTreeProvider;
        }

        public async Task<IEnumerable<FrequentItemsetDto>> Handle(ExtractFrequentItemsetsQuery request, CancellationToken cancellationToken)
        {
            var subjects = await _subjectsService.GetOrderedSubjects();
            var tree = await _fpTreeProvider.GetActualFpTree();

            var result = new List<FrequentItemsetDto>();
            //var resultSet = new HashSet<FrequentItemsetModel>();

            foreach (var product in subjects)
            {
                //Условный базис
                var conditionalBase = tree.Where(a => a.SubjectId == product.Id)
                    .Select(a => a.PathToRoot.Select(n =>
                        new
                        {
                            ProductId = n.Node.SubjectId.Value,
                            Product = n.Node.Subject,
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
                        if (support < request._minSupport)
                            continue;

                        var set = CollectFrequentItemsets(item.Item, support, totalSups);

                        //Фильтрация повторов
                        if (result.Any(a => a.Subjects.Intersect(set).Count() == set.Count()))
                            continue;

                        result.Add(new FrequentItemsetDto
                        {
                            Support = support,
                            Subjects = set
                        });
                    }
                }
            }

            return result;
        }
        
        /// <summary>
        /// Выборка частых наборов из ветви условного базиса
        /// </summary>
        private IEnumerable<Subject> CollectFrequentItemsets(ConditionalTreeItem root, int support, Dictionary<Guid, int> totalSups)
        {
            var node = root;
            do
            {
                if (totalSups.TryGetValue(node.Node.SubjectId.Value, out int s) && s < support) continue;

                yield return node.Node.Subject;
            } while ((node = node.Next) != null);
        }
    }

    #endregion
}