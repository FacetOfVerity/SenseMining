using System.Collections.Generic;

namespace SenseMining.Domain.MessageContracts
{
    public interface ITransactionMessage
    {
        IEnumerable<string> Items { get; set; }
    }
}
