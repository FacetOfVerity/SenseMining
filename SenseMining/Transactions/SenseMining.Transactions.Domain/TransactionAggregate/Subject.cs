namespace SenseMining.Transactions.Domain.TransactionAggregate;

public class Subject
{
    public Guid Id { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public string Name { get; set; }

    public int Support { get; set; }

    public virtual SubjectCategory Category { get; set; }

    protected Subject() { }

    public Subject(string name)
    {
        Id = Guid.NewGuid();
        Support = 1;
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}