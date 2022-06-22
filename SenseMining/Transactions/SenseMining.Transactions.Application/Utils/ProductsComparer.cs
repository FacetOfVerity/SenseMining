using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Application.Utils;

public class SubjectsComparer : IComparer<Guid>
{
    private readonly Dictionary<Guid, int> _order;

    public SubjectsComparer(List<Subject> order)
    {
        _order = new Dictionary<Guid, int>();

        for (int i = 0; i < order.Count; i++)
        {
            _order.Add(order[i].Id, i);
        }
    }

    public int Compare(Guid x, Guid y)
    {
        if (x == y)
        {
            return 0;
        }

        return _order[x] > _order[y] ? 1 : -1;
    }
}