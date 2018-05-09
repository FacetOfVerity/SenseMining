using System;
using System.Collections.Generic;
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
        }
    }
}
