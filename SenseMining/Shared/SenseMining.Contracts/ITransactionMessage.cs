namespace SenseMining.Contracts;

public interface ITransactionMessage
{
    IEnumerable<string> Items { get; set; }
}