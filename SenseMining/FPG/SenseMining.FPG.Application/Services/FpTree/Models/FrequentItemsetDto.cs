using SenseMining.FPG.Domain.FpTreeAggregate;

namespace SenseMining.FPG.Application.Services.FpTree.Models;

public class FrequentItemsetDto
{
    public int Support { get; set; }

    public IEnumerable<Subject> Subjects { get; set; }
}