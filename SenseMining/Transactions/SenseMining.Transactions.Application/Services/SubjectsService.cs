using Microsoft.EntityFrameworkCore;
using SenseMining.Transactions.Application.Abstractions;
using SenseMining.Transactions.Domain.TransactionAggregate;

namespace SenseMining.Transactions.Application.Services;

public class SubjectsService : ISubjectsService
{
    private readonly ITransactionsDbContext _dbContext;
    private readonly CancellationToken _cancellationToken;

    public SubjectsService(ITransactionsDbContext dbContext, CancellationTokenSource cancellationTokenSource)
    {
        _dbContext = dbContext;
        _cancellationToken = cancellationTokenSource.Token;
    }

    public async Task<List<Subject>> DefineTransactionSubjects(IEnumerable<string> subjects)
    {
        return await _dbContext.Subjects.AsQueryable()
            .Where(a => subjects.Contains(a.Name))
            .ToListAsync(_cancellationToken);
    }

    public async Task<List<Subject>> InsertSubjects(IEnumerable<string> subjects,
        bool saveImmediately)
    {
        var subjectsList = subjects.Select(a => new Subject(a)).ToList();
        subjectsList.ForEach(a => _dbContext.Subjects.Add(a));

        if (saveImmediately)
        {
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }

        return subjectsList;
    }

    public async Task IncrementFrequencies(IEnumerable<Guid> subjectsIds,
        bool saveImmediately)
    {
        var subjects = await _dbContext.Subjects.AsQueryable()
            .Where(a =>subjectsIds.Contains(a.Id))
            .ToListAsync(_cancellationToken);
        subjects.ForEach(a => a.Support++);

        if (saveImmediately)
        {
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
    }

    public async Task<List<Subject>> GetOrderedSubjects()
    {
        return await _dbContext.Subjects.OrderByDescending(a => a.Support).AsNoTracking()
            .ToListAsync(_cancellationToken);
    }
}