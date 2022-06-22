using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Application.Services;

public interface ISubjectsService
{
    Task<List<Subject>> DefineTransactionSubjects(IEnumerable<string> subjects);

    Task<List<Subject>> InsertSubjects(IEnumerable<string> subjects, bool saveImmediately);

    Task IncrementFrequencies(IEnumerable<Guid> subjectsIds, bool saveImmediately);

    Task<List<Subject>> GetOrderedSubjects();
}