namespace Common.Application.Abstractions;

public interface IContext
{
    Task<int> SaveChangesAsync(CancellationToken token);
}