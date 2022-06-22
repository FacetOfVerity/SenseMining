using MediatR;
using SenseMining.FPG.Application.Services.FpTree;
using SenseMining.FPG.Application.Services.FpTree.Models;

namespace SenseMining.FPG.Application.Features;

/// <summary>
/// Query extracts actual FP Tree.
/// </summary>
public class ActualFpTreeQuery : IRequest<FpTreeModel>
{
    #region Handler

    /// <inheritdoc />
    public class Handler : IRequestHandler<ActualFpTreeQuery, FpTreeModel>
    {
        private readonly IFpTreeProvider _fpTreeProvider;

        /// <summary/>
        public Handler(IFpTreeProvider fpTreeProvider)
        {
            _fpTreeProvider = fpTreeProvider;
        }

        public async Task<FpTreeModel> Handle(ActualFpTreeQuery request, CancellationToken cancellationToken)
        {
            return await _fpTreeProvider.GetFpTreeModel();
        }
    }

    #endregion
}