﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SenseMining.Domain.Services.FpTree.Models;

namespace SenseMining.Domain.Services.FpTree
{
    public interface IFpTreeService
    {
        Task<FpTreeModel> GetTreeFromDatabase();

        Task UpdateTree();

        Task<List<FrequentItemsetModel>> GetFrequentItemsets(int minSupport);
    }
}