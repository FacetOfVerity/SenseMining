using System;
using System.Threading.Tasks;

namespace SenseMining.Worker.Abstractions
{
    public interface IJob
    {
        TimeSpan Interval { get; }
        Task Execute();
    }
}
