namespace SenseMining.Transactions.Domain.TransactionAggregate;

public class Transaction
{
    public Guid Id { get; set; }

    public DateTimeOffset CreationTime { get; set; }

    public ICollection<TransactionItem> Items { get; set; }

    public Transaction()
    {
        CreationTime = DateTimeOffset.UtcNow;
        Id = Guid.NewGuid();
    }
}