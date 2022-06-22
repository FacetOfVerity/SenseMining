using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Application.Services.FpTree.Internal;

internal class InternalFrequentItemsetModel
{
    public Guid ProductId { get; set; }

    public Subject Subject { get; set; }

    public int Support { get; set; }

    public int ConditionalSupport { get; set; }
}