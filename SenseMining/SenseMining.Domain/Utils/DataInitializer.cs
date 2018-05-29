using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SenseMining.Database;
using SenseMining.Domain.Services;

namespace SenseMining.Domain.Utils
{
    public class DataInitializer
    {
        private readonly ITransactionsService _transactionsService;
        private readonly DatabaseContext _dbContext;

        public DataInitializer(ITransactionsService transactionsService, DatabaseContext dbContext)
        {
            _transactionsService = transactionsService;
            _dbContext = dbContext;
        }

        public async Task Initialize()
        {
            if (! await _dbContext.Transactions.AnyAsync())
            {
                await MushroomData();
            }
        }

        private async Task SimpleExample()
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

        private async Task MushroomData()
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
}
