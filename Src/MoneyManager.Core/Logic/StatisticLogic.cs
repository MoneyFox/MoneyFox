using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cirrious.CrossCore;
using MoneyManager.Core.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Logic
{
    public class StatisticLogic
    {
        /// <summary>
        ///     Get a list with income, spending and earnings for custom date range
        /// </summary>
        /// <returns>List with income, spending and earning item.</returns>
        public static ObservableCollection<StatisticItem> GetMonthlyCashFlow(DateTime startDate, DateTime endDate)
        {
            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    AllTransaction
                        .Where(x => x.Type != (int) TransactionType.Transfer)
                        .Where(x => x.Date >= startDate.Date && x.Date <= endDate.Date)
                        .ToList());

            return GetCashFlowStatisticItems(transactionListFunc);
        }

        /// <summary>
        ///     Get a list with income, spending and earnings for current month
        /// </summary>
        /// <returns>List with income, spending and earning item.</returns>
        public static ObservableCollection<StatisticItem> GetMonthlyCashFlow()
        {
            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    AllTransaction
                        .Where(x => x.Type != (int) TransactionType.Transfer)
                        .Where(x => x.Date.Month == DateTime.Now.Month)
                        .ToList());

            return GetCashFlowStatisticItems(transactionListFunc);
        }

        private static ObservableCollection<StatisticItem> GetCashFlowStatisticItems(
            Func<List<FinancialTransaction>> getTransactionListFunc)
        {
            var transactionList = getTransactionListFunc();

            var itemList = new ObservableCollection<StatisticItem>();

            var income = new StatisticItem
            {
                Category = Strings.RevenueLabel,
                Value = transactionList.Where(x => x.Type == (int) TransactionType.Income).Sum(x => x.Amount)
            };
            income.Label = income.Category + ": " +
                           Math.Round(income.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            var spent = new StatisticItem
            {
                Category = Strings.ExpenseLabel,
                Value = transactionList.Where(x => x.Type == (int) TransactionType.Spending).Sum(x => x.Amount)
            };
            spent.Label = spent.Category + ": " +
                          Math.Round(spent.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            var increased = new StatisticItem
            {
                Category = Strings.IncreaseLabel,
                Value = income.Value - spent.Value
            };
            increased.Label = increased.Category + ": " +
                              Math.Round(increased.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            itemList.Add(income);
            itemList.Add(spent);
            itemList.Add(increased);

            return itemList;
        }

        /// <summary>
        ///     Returns spreading with custom date range
        /// </summary>
        /// <param name="startDate">minimum date</param>
        /// <param name="endDate">max date</param>
        /// <returns>List with statistic items.</returns>
        public static ObservableCollection<StatisticItem> GetSpreading(DateTime startDate, DateTime endDate)
        {
            if (AllTransaction == null)
            {
                TransactionData.LoadList();
            }

            if (AllCategories == null)
            {
                CateogryData.LoadList();
            }

            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    AllTransaction
                        .Where(x => x.Category != null)
                        .Where(x => x.Date >= startDate.Date && x.Date <= endDate.Date)
                        .Where(x => x.Type == (int) TransactionType.Spending)
                        .ToList());

            return GetSpreadingStatisticItems(transactionListFunc);
        }

        /// <summary>
        ///     returns the spreading of the current month
        /// </summary>
        /// <returns>List with statistic items.</returns>
        public static ObservableCollection<StatisticItem> GetSpreading()
        {
            if (AllTransaction == null)
            {
                TransactionData.LoadList();
            }

            if (AllCategories == null)
            {
                CateogryData.LoadList();
            }

            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    AllTransaction
                        .Where(x => x.Category != null)
                        .Where(x => x.Date.Month == DateTime.Today.Date.Month)
                        .Where(x => x.Type == (int) TransactionType.Spending)
                        .ToList());

            return GetSpreadingStatisticItems(transactionListFunc);
        }

        private static ObservableCollection<StatisticItem> GetSpreadingStatisticItems(
            Func<List<FinancialTransaction>> getTransactionListFunc)
        {
            var transactionList = getTransactionListFunc();

            var tempStatisticList = AllCategories.Select(category => new StatisticItem
            {
                Category = category.Name,
                Value = transactionList
                    .Where(x => x.Category.Id == category.Id)
                    .Sum(x => x.Amount)
            }).ToList();

            RemoveNullList(tempStatisticList);

            tempStatisticList = tempStatisticList.OrderByDescending(x => x.Value).ToList();
            var statisticList = tempStatisticList.Take(6).ToList();

            AddOtherItem(tempStatisticList, statisticList);

            IncludeIncome(statisticList);

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
            item.Label = item.Category + ": " + item.Value + " " + Settings.DefaultCurrency;
        }

        private static void IncludeIncome(IEnumerable<StatisticItem> statisticList)
        {
            foreach (var statisticItem in statisticList)
            {
                statisticItem.Value -= AllTransaction
                    .Where(x => x.Type == (int) TransactionType.Income)
                    .Where(x => x.Category != null)
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
            if (statisticList.Count < 6)
            {
                return;
            }

            var othersItem = new StatisticItem
            {
                Category = "Others",
                Value = tempStatisticList
                    .Where(x => !statisticList.Contains(x))
                    .Sum(x => x.Value)
            };
            othersItem.Label = othersItem.Category + ": " + othersItem.Value + " " + Settings.DefaultCurrency;

            if (othersItem.Value > 0)
            {
                statisticList.Add(othersItem);
            }
        }

        /// <summary>
        ///     Returns a list with a summary per category for the selected date range.
        /// </summary>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">enddate</param>
        /// <returns>List with statistic Items.</returns>
        public static ObservableCollection<StatisticItem> GetCategorySummary(DateTime startDate, DateTime endDate)
        {
            var categories = new ObservableCollection<StatisticItem>();

            foreach (var category in AllCategories)
            {
                categories.Add(new StatisticItem
                {
                    Category = category.Name,
                    Value = AllTransaction
                        .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date)
                        .Where(x => x.CategoryId == category.Id)
                        .Where(x => x.Type != (int) TransactionType.Transfer)
                        .Sum(x => x.Type == (int) TransactionType.Spending
                            ? -x.Amount
                            : x.Amount),
                    Label = Mvx.Resolve<SettingDataAccess>().DefaultCurrency
                });
            }

            return new ObservableCollection<StatisticItem>(
                categories.Where(x => Math.Abs(x.Value) > 0.1).OrderBy(x => x.Value).ToList());
        }

        #region Properties

        private static IEnumerable<FinancialTransaction> AllTransaction => Mvx.Resolve<ITransactionRepository>().Data;

        private static IEnumerable<Category> AllCategories => Mvx.Resolve<IRepository<Category>>().Data;

        private static IDataAccess<FinancialTransaction> TransactionData => Mvx.Resolve<IDataAccess<FinancialTransaction>>();

        private static IDataAccess<Category> CateogryData => Mvx.Resolve<IDataAccess<Category>>();

        private static SettingDataAccess Settings => Mvx.Resolve<SettingDataAccess>();

        #endregion
    }
}