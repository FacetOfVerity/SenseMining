namespace SenseMining.Transactions.Domain.TransactionAggregate;

/// <summary>
/// Категория продукта
/// </summary>
public class SubjectCategory
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    /// <summary/>
    public SubjectCategory()
    {
        Id = Guid.NewGuid();
    }

    /// <summary/>
    public SubjectCategory(string name) : this()
    {
        Name = name;
    }
}