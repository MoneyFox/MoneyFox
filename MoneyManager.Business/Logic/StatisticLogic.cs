#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Windows.Foundation.Metadata;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using Telerik.Charting;

#endregion

namespace MoneyManager.Business.Logic
{
    public class StatisticLogic
    {
        private static IEnumerable<FinancialTransaction> allTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().AllTransactions; }
        }
        
        private static IEnumerable<Category> allCategories
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>().AllCategories; }
        }

        private static TransactionDataAccess transactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public static ObservableCollection<StatisticItem> GetMonthlyCashFlow()
        {
            var transactionList = allTransaction
                .Where(x => x.Date.Month == DateTime.Now.Month)
                .ToList();

            var itemList = new ObservableCollection<StatisticItem>();

            var income = new StatisticItem
            {
                Category = Translation.GetTranslation("IncomeLabel"),
                Value = transactionList.Where(x => x.Type == (int) TransactionType.Income).Sum(x => x.Amount)
            };

            var spent = new StatisticItem
            {
                Category = Translation.GetTranslation("SpentLabel"),
                Value = transactionList.Where(x => x.Type == (int) TransactionType.Spending).Sum(x => x.Amount)
            };

            var increased = new StatisticItem
            {
                Category = Translation.GetTranslation("IncreasedLabel"),
                Value = income.Value - spent.Value
            };

            itemList.Add(income);
            itemList.Add(spent);
            itemList.Add(increased);

            return itemList;
        }

        public static ObservableCollection<StatisticItem> GetSpreading()
        {
            if (allTransaction == null)
            {
                transactionData.LoadList();
            }

            var transactionList = allTransaction
                .Where(x => x.Category != null
                            && x.Cleared
                            && x.Date.Month == DateTime.Today.Date.Month)
                .ToList();

            var tempStatisticList = allCategories.Select(category => new StatisticItem
            {
                Category = category.Name,
                Value = transactionList
                    .Where(x => x.Category == category)
                    .Sum(x => x.Amount),
            }).ToList();

            tempStatisticList = tempStatisticList.OrderByDescending(x => x.Value).ToList();

            var statisticList = tempStatisticList.Take(4).ToList();
            statisticList.Add(new StatisticItem
            {
                Category = "Others",
                Value = tempStatisticList
                    .Where(x => !statisticList.Contains(x))
                    .Sum(x => x.Value)
            });

            return new ObservableCollection<StatisticItem>(statisticList);
        }
    }
}