using MediatR;
using SenseMining.FPG.Application.Services.FpTree;
using SenseMining.FPG.Application.Services.FpTree.Models;

namespace SenseMining.FPG.Application.Features;

/// <summary>
/// Query extracts frequent itemsets from actual FP Tree.
/// </summary>
public class FrequentItemsetsQuery : IRequest<IEnumerable<FrequentItemsetDto>>
{
    #region Data

    private readonly int _minSupport;

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="minSupport">Min support.</param>
    public FrequentItemsetsQuery(int minSupport)
    {
        _minSupport = minSupport;
    }

    #endregion

    #region Handler

    /// <inheritdoc />
    public class Handler : IRequestHandler<FrequentItemsetsQuery, IEnumerable<FrequentItemsetDto>>
    {
        private readonly IFpTreeService _fpTreeService;

        /// <summary/>
        public Handler(IFpTreeService fpTreeService)
        {
            _fpTreeService = fpTreeService;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FrequentItemsetDto>> Handle(FrequentItemsetsQuery request,
            CancellationToken cancellationToken)
        {
            return await _fpTreeService.ExtractFrequentItemsets(request._minSupport);
        }
    }

    #endregion
}