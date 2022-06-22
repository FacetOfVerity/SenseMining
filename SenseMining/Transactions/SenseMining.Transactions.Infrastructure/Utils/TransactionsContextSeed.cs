using SenseMining.Transactions.Application.Services;

namespace SenseMining.Transactions.Infrastructure.Utils;

public class TransactionsContextSeed
{
    private readonly ITransactionsService _transactionsService;

    public TransactionsContextSeed(ITransactionsService transactionsService)
    {
        _transactionsService = transactionsService;
    }

    public async Task SimpleExample()
    {
        await _transactionsService.InsertTransaction(new List<string>
        {
            "a",
            "b",
            "c",
            "d",
            "e"
        });
        await _transactionsService.InsertTransaction(new List<string>
        {
            "a",
            "b",
            "c"
        });
        await _transactionsService.InsertTransaction(new List<string>
        {
            "a",
            "c",
            "d",
            "e"
        });
        await _transactionsService.InsertTransaction(new List<string>
        {
            "b",
            "c",
            "d",
            "e"
        });
        await _transactionsService.InsertTransaction(new List<string>
        {
            "b",
            "c"
        });
        await _transactionsService.InsertTransaction(new List<string>
        {
            "b",
            "d",
            "e"
        });
        await _transactionsService.InsertTransaction(new List<string>
        {
            "c",
            "d",
            "e"
        });
    }

    public async Task MushroomData()
    {
        using (var reader = File.OpenText(Path.Combine("Resourses", "mushroom.dat")))
        {
            var line = string.Empty;
            var transaction = new List<string>();
            var count = 0;
            while ((line = reader.ReadLine()) != null)
            {
                transaction = new List<string>(line.Trim().Split(' ')).Select(s => s.Trim()).ToList();
                await _transactionsService.InsertTransaction(transaction);

                count++;

                if (count == 200) break;
            }
        }
    }
}