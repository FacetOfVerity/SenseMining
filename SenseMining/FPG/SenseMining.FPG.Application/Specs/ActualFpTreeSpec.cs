using Ardalis.Specification;
using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Application.Specs;

public class ActualFpTreeSpec : Specification<Node>
{
    public ActualFpTreeSpec()
    {
        Query.Include(a => a.Parent).Include(a => a.Subject);
    }
}