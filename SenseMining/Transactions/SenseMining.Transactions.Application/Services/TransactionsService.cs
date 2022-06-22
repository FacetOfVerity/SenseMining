using Microsoft.EntityFrameworkCore;
using SenseMining.Transactions.Application.Abstractions;
using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Application.Services;

public class TransactionsService : ITransactionsService
{
    private readonly ITransactionsDbContext _dbContext;
    private readonly CancellationToken _cancellationToken;
    private readonly ISubjectsService _subjectsService;

    public TransactionsService(ITransactionsDbContext dbContext, CancellationTokenSource cancellationTokenSource,
        ISubjectsService subjectsService)
    {
        _dbContext = dbContext;
        _subjectsService = subjectsService;
        _cancellationToken = cancellationTokenSource.Token;
    }

    public async Task InsertTransaction(IEnumerable<string> transactionItems)
    {
        var existing = await _subjectsService.DefineTransactionSubjects(transactionItems);
        await _subjectsService.IncrementFrequencies(existing.Select(a => a.Id), false);

        var newSubjects = await RegisterNewSubjects(transactionItems, existing);
        newSubjects.AddRange(existing);

        var transaction = new Transaction();
        var items = newSubjects.Select(a => new TransactionItem(transaction.Id, a.Id));
        foreach (var transactionItem in items)
        {
            _dbContext.TransactionItems.Add(transactionItem);
        }

        _dbContext.Transactions.Add(transaction);

        await _dbContext.SaveChangesAsync(_cancellationToken);
    }

    private async Task<List<Subject>> RegisterNewSubjects(IEnumerable<string> all,
        List<Subject> existing)
    {
        var newSubjects = all.Except(existing.Select(a => a.Name));

        return await _subjectsService.InsertSubjects(newSubjects.ToList(), false);
    }

    public async Task<List<Transaction>> GetLastTransactions(DateTimeOffset dateFrom)
    {
        return await _dbContext.Transactions
            .Include(a => a.Items).ThenInclude(a => a.Subject)
            .Where(a => a.CreationTime >= dateFrom).AsNoTracking().ToListAsync(_cancellationToken);
    }

}