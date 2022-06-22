namespace SenseMining.Transactions.Domain.TransactionAggregate;

public class TransactionItem
{
    public Guid TransactionId { get; set; }
    
    public Guid SubjectId { get; set; }

    public Transaction Transaction { get; set; }

    public Subject Subject { get; set; }

    public TransactionItem()
    {
            
    }

    public TransactionItem(Guid transactionId, Guid subjectId)
    {
        TransactionId = transactionId;
        SubjectId = subjectId;
    }
}