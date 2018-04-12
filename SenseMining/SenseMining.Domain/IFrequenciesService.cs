using System.Collections.Generic;
using System.Threading.Tasks;

namespace SenseMining.Domain
{
    public interface IFrequenciesService
    {
        Task IncrementFrequencies(List<int> productsIds, bool saveImmediately);
    }
}
