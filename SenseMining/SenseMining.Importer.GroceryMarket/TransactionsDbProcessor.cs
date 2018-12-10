﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChoETL;
using MassTransit;
using SenseMining.Domain.MessageContracts;
using SenseMining.Importer.GroceryMarket.Models;

namespace SenseMining.Importer.GroceryMarket
{
    public class TransactionsDbProcessor
    {

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ImporterOptions _options;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly Dictionary<int, string> _procucts;
        private readonly Dictionary<int, string> _procuctCategories;

        public TransactionsDbProcessor(CancellationTokenSource cancellationTokenSource, ImporterOptions options, IPublishEndpoint publishEndpoint)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _options = options;
            _publishEndpoint = publishEndpoint;
            _procucts = new Dictionary<int, string>();
            _procuctCategories = new Dictionary<int, string>();
        }

        public async Task Start()
        {

            using (var reader = new ChoCSVReader<CategoryModel>(Path.Combine(_options.ResourcesPath, "categories.csv")).WithFirstLineHeader())
            {
                foreach (var categoryModel in reader)
                {
                    _procuctCategories.Add(categoryModel.CategoryID, categoryModel.CategoryName);
                }
            }

            using (var reader = new ChoCSVReader<ProductModel>(Path.Combine(_options.ResourcesPath, "products.csv")).WithFirstLineHeader())
            {
                foreach (var productModel in reader)
                {
                    _procucts.Add(productModel.ProductID,
                        $"{productModel.ProductName};{_procuctCategories[productModel.CategoryID]}");
                }
            }

            using (var reader = new ChoCSVReader<SalesRowModel>(Path.Combine(_options.ResourcesPath, "sales", "sales-000.csv")).WithFirstLineHeader())
            {
                var groupsByCustomer = reader.Take(300).Where(a => a.SalesDate.HasValue).GroupBy(a => a.CustomerID);
                var groupsByCustomerByDate = groupsByCustomer.Select(a => new { CustomerID = a.Key, Days = a.GroupBy(b => b.SalesDate.Value.Date) }).Where(a => a.Days.Any()).ToArray();

                foreach (var groupByCustomer in groupsByCustomerByDate)
                {
                    foreach (var groupByDate in groupByCustomer.Days)
                    {
                        await _publishEndpoint.Publish<ITransactionMessage>(new
                        {
                            Items = groupByDate.Select(a => _procucts[a.ProductID])
                        });
                    }
                }
            }
        }
    }
}