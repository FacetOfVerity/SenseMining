using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using SenseMining.Domain.MessageContracts;
using SenseMining.Domain.TransactionsProcessing;

namespace SenseMining.Listener
{
    public class TransactionsQueueConsumer : IConsumer<ITransactionMessage>
    {
        private readonly ITransactionsProcessor _transactionsProcessor;

        public TransactionsQueueConsumer(ITransactionsProcessor transactionsProcessor)
        {
            _transactionsProcessor = transactionsProcessor;
        }

        public async Task Consume(ConsumeContext<ITransactionMessage> context)
        {
            await _transactionsProcessor.ReceiveTransaction(context.Message.Items);
        }
    }
}
