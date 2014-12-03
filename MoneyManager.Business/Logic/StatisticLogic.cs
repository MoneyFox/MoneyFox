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
        #region Properties
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
        private static SettingDataAccess settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }
        #endregion

        public static ObservableCollection<StatisticItem> GetMonthlyCashFlow()
        {
            var transactionList = allTransaction
                .Where(x => x.Type != (int) TransactionType.Transfer)
                .Where(x => x.Date.Month == DateTime.Now.Month)
                .ToList();

            var itemList = new ObservableCollection<StatisticItem>();

            var income = new StatisticItem
            {
                Category = Translation.GetTranslation("RevenuesLabel"),
                Value = transactionList.Where(x => x.Type == (int) TransactionType.Income).Sum(x => x.Amount)
            };
            income.Label = income.Category + ": " + income.Value + " " + settings.DefaultCurrency;

            var spent = new StatisticItem
            {
                Category = Translation.GetTranslation("ExpensesLabel"),
                Value = transactionList.Where(x => x.Type == (int) TransactionType.Spending).Sum(x => x.Amount)
            };
            spent.Label = spent.Category + ": " + spent.Value + " " + settings.DefaultCurrency;

            var increased = new StatisticItem
            {
                Category = Translation.GetTranslation("IncreasesLabel"),
                Value = income.Value - spent.Value
            };
            increased.Label = increased.Category + ": " + increased.Value + " " + settings.DefaultCurrency;

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
                            && x.Date.Month == DateTime.Today.Date.Month
                            && x.Type == (int) TransactionType.Spending)
                .ToList();

            var tempStatisticList = allCategories.Select(category => new StatisticItem
            {
                Category = category.Name,
                Value = transactionList
                    .Where(x => x.Category == category)
                    .Sum(x => x.Amount),
            }).ToList();

            RemoveNullList(tempStatisticList);

            tempStatisticList = tempStatisticList.OrderByDescending(x => x.Value).ToList();
            var statisticList = tempStatisticList.Take(6).ToList();

            AddOtherItem(tempStatisticList, statisticList);

            IncludeIncome(statisticList, transactionList);

            return new ObservableCollection<StatisticItem>(statisticList);
        }

        private static void RemoveNullList(ICollection<StatisticItem> tempStatisticList)
        {
            var nullerList = tempStatisticList.Where(x => x.Value == 0).ToList();
            foreach (var statisticItem in nullerList)
            {
                tempStatisticList.Remove(statisticItem);
            }
        }

        private static void SetLabel(StatisticItem item)
        {
            item.Label = item.Category + ": " + item.Value + " " + settings.DefaultCurrency;
        }

        private static void IncludeIncome(IEnumerable<StatisticItem> statisticList, List<FinancialTransaction> transactionList)
        {
            foreach (var statisticItem in statisticList)
            {
                statisticItem.Value -= allTransaction
                    .Where(x => x.Type == (int) TransactionType.Income)
                    .Where(x => x.Category.Name == statisticItem.Category)
                    .Sum(x => x.Amount);

                SetLabel(statisticItem);

                if (statisticItem.Value <= 0)
                {
                    statisticItem.Value = 0;
                }
            }
        }

        private static void AddOtherItem(IEnumerable<StatisticItem> tempStatisticList,
            ICollection<StatisticItem> statisticList)
        {
            if (statisticList.Count < 6) return;

            var othersItem = new StatisticItem
            {
                Category = "Others",
                Value = tempStatisticList
                    .Where(x => !statisticList.Contains(x))
                    .Sum(x => x.Value)
            };
            othersItem.Label = othersItem.Category + ": " + othersItem.Value + " " + settings.DefaultCurrency;

            if (othersItem.Value > 0)
            {
                statisticList.Add(othersItem);
            }
        }
    }
}
