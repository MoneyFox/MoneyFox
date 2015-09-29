using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;

namespace MoneyManager.Core.Manager
{
    public class StatisticManager
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly ITransactionRepository transactionRepository;

        public StatisticManager(ITransactionRepository transactionRepository,
            IRepository<Category> categoryRepository)
        {
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        ///     Get a list with income, spending and earnings for custom date range
        /// </summary>
        /// <returns>List with income, spending and earning item.</returns>
        public List<StatisticItem> GetMonthlyCashFlow(DateTime startDate, DateTime endDate)
        {
            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    transactionRepository.Data
                        .Where(x => x.Type != (int) TransactionType.Transfer)
                        .Where(x => x.Date >= startDate.Date && x.Date <= endDate.Date)
                        .ToList());

            return GetCashFlowStatisticItems(transactionListFunc);
        }

        /// <summary>
        ///     Get a list with income, spending and earnings for current month
        /// </summary>
        /// <returns>List with income, spending and earning item.</returns>
        public List<StatisticItem> GetMonthlyCashFlow()
        {
            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    transactionRepository.Data
                        .Where(x => x.Type != (int) TransactionType.Transfer)
                        .Where(x => x.Date.Month == DateTime.Now.Month)
                        .ToList());

            return GetCashFlowStatisticItems(transactionListFunc);
        }

        private List<StatisticItem> GetCashFlowStatisticItems(
            Func<List<FinancialTransaction>> getTransactionListFunc)
        {
            var transactionList = getTransactionListFunc();

            var itemList = new List<StatisticItem>();

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
        public List<StatisticItem> GetSpreading(DateTime startDate, DateTime endDate)
        {
            if (transactionRepository.Data == null)
            {
                transactionRepository.Load();
            }

            if (categoryRepository.Data == null)
            {
                categoryRepository.Load();
            }

            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    transactionRepository.Data
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
        public List<StatisticItem> GetSpreading()
        {
            if (transactionRepository.Data == null)
            {
                transactionRepository.Load();
            }

            if (categoryRepository.Data == null)
            {
                categoryRepository.Load();
            }

            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    transactionRepository.Data
                        .Where(x => x.Category != null)
                        .Where(x => x.Date.Month == DateTime.Today.Date.Month)
                        .Where(x => x.Type == (int) TransactionType.Spending)
                        .ToList());

            return GetSpreadingStatisticItems(transactionListFunc);
        }

        private List<StatisticItem> GetSpreadingStatisticItems(
            Func<List<FinancialTransaction>> getTransactionListFunc)
        {
            var transactionList = getTransactionListFunc();

            var tempStatisticList = categoryRepository.Data.Select(category => new StatisticItem
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

            return statisticList;
        }

        private void RemoveNullList(ICollection<StatisticItem> tempStatisticList)
        {
            var nullerList = tempStatisticList.Where(x => Math.Abs(x.Value) < 0.001).ToList();
            foreach (var statisticItem in nullerList)
            {
                tempStatisticList.Remove(statisticItem);
            }
        }

        private void SetLabel(StatisticItem item)
        {
            item.Label = item.Category + ": " + item.Value;
        }

        private void IncludeIncome(IEnumerable<StatisticItem> statisticList)
        {
            foreach (var statisticItem in statisticList)
            {
                statisticItem.Value -= transactionRepository.Data
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

        private void AddOtherItem(IEnumerable<StatisticItem> tempStatisticList,
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
            othersItem.Label = othersItem.Category + ": " + othersItem.Value;

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
        public ObservableCollection<StatisticItem> GetCategorySummary(DateTime startDate, DateTime endDate)
        {
            var categories = new ObservableCollection<StatisticItem>();

            foreach (var category in categoryRepository.Data)
            {
                categories.Add(new StatisticItem
                {
                    Category = category.Name,
                    Value = transactionRepository.Data
                        .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date)
                        .Where(x => x.CategoryId == category.Id)
                        .Where(x => x.Type != (int) TransactionType.Transfer)
                        .Sum(x => x.Type == (int) TransactionType.Spending
                            ? -x.Amount
                            : x.Amount)
                });
            }

            return new ObservableCollection<StatisticItem>(
                categories.Where(x => Math.Abs(x.Value) > 0.1).OrderBy(x => x.Value).ToList());
        }
    }
}