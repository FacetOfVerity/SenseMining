using MediatR;
using Microsoft.AspNetCore.Mvc;
using SenseMining.FPG.Application.Features;
using SenseMining.FPG.Application.Services.FpTree.Models;

namespace SenseMining.FPG.API.Controllers;

/// <summary>
/// API for interaction with FP Tree.
/// </summary>
[Route("FpTree")]
public class FpTreeController : Controller
{
    private readonly IMediator _mediator;

    /// <summary/>
    public FpTreeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получение модели FP-дерева (Json)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<FpTreeModel> GetFpTree()
    {
        return await _mediator.Send(new ActualFpTreeQuery());
    }

    /// <summary>
    /// Запуск обновления FP-дерева
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateTree()
    {
        await _mediator.Send(new UpdateTreeCommand());
    }

    /// <summary>
    /// Extract frequent itemsets from actual FP Tree
    /// </summary>
    /// <param name="minSupport">Min support.</param>
    [HttpGet]
    [Route("FrequentItemsets")]
    public async Task<object> GetFrequentItemsets([FromQuery] int minSupport)
    {
        var result = await _mediator.Send(new FrequentItemsetsQuery(minSupport));
        return new //TODO Mapper
        {
            Count = result.Count(),
            Data = result.Select(a => new
            {
                a.Support,
                Subjects = a.Subjects.Select(p => p.Id)
            })
        };
    }

    /// <summary>
    /// Получение ассоциативных правил для заданного значения надежности
    /// </summary>
    /// <param name="minSupport"></param>
    /// <param name="minconf"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("AssociationRules")]
    public async Task<object> GetAssociationRules([FromQuery] int minSupport, [FromQuery]int minconf)
    {
        var result = await _mediator.Send(new ExtractFrequentItemsetsQuery(minSupport));
        
        return new
        {
            Count = result.Count(),
            Data = result.Select(a => new
            {
                a.Support,
                Subjects = a.Subjects.Select(p => p.Id)
            })
        };
    }
}