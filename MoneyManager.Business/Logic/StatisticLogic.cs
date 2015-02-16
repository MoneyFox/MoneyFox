#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

#endregion

namespace MoneyManager.Business.Logic {
    public class StatisticLogic {
        #region Properties

        private static IEnumerable<FinancialTransaction> allTransaction {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().AllTransactions; }
        }

        private static IEnumerable<Category> allCategories {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>().AllCategories; }
        }

        private static TransactionDataAccess transactionData {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        private static CategoryDataAccess cateogryData {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        private static SettingDataAccess settings {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        #endregion

        /// <summary>
        /// Get a list with income, spending and earnings for custom date range
        /// </summary>
        /// <returns>List with income, spending and earning item.</returns>
        public static ObservableCollection<StatisticItem> GetMonthlyCashFlow(DateTime startDate, DateTime endDate) {
            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    allTransaction
                        .Where(x => x.Type != (int) TransactionType.Transfer)
                        .Where(x => x.Date >= startDate.Date && x.Date <= endDate.Date)
                        .ToList());

            return GetCashFlowStatisticItems(transactionListFunc);
        }

        /// <summary>
        /// Get a list with income, spending and earnings for current month
        /// </summary>
        /// <returns>List with income, spending and earning item.</returns>
        public static ObservableCollection<StatisticItem> GetMonthlyCashFlow() {
            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    allTransaction
                        .Where(x => x.Type != (int) TransactionType.Transfer)
                        .Where(x => x.Date.Month == DateTime.Now.Month)
                        .ToList());

            return GetCashFlowStatisticItems(transactionListFunc);
        }

        private static ObservableCollection<StatisticItem> GetCashFlowStatisticItems(
            Func<List<FinancialTransaction>> getTransactionListFunc) {
            List<FinancialTransaction> transactionList = getTransactionListFunc();

            var itemList = new ObservableCollection<StatisticItem>();

            var income = new StatisticItem {
                Category = Translation.GetTranslation("RevenuesLabel"),
                Value = transactionList.Where(x => x.Type == (int) TransactionType.Income).Sum(x => x.Amount)
            };
            income.Label = income.Category + ": " + Math.Round(income.Value, 2, MidpointRounding.AwayFromZero).ToString("C") + " " + settings.DefaultCurrency;

            var spent = new StatisticItem {
                Category = Translation.GetTranslation("ExpensesLabel"),
                Value = transactionList.Where(x => x.Type == (int) TransactionType.Spending).Sum(x => x.Amount)
            };
            spent.Label = spent.Category + ": " + Math.Round(spent.Value, 2, MidpointRounding.AwayFromZero).ToString("C") + " " + settings.DefaultCurrency;

            var increased = new StatisticItem {
                Category = Translation.GetTranslation("IncreasesLabel"),
                Value = income.Value - spent.Value
            };
            increased.Label = increased.Category + ": " + Math.Round(increased.Value, 2, MidpointRounding.AwayFromZero).ToString("C") + " " +
                              settings.DefaultCurrency;

            itemList.Add(income);
            itemList.Add(spent);
            itemList.Add(increased);

            return itemList;
        }

        public static ObservableCollection<StatisticItem> GetSpreading(DateTime startDate, DateTime endDate) {
            if (allTransaction == null) {
                transactionData.LoadList();
            }

            if (allCategories == null) {
                cateogryData.LoadList();
            }

            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    allTransaction
                        .Where(x => x.Category != null)
                        .Where(x => x.Date >= startDate.Date && x.Date <= endDate.Date)
                        .Where(x => x.Type == (int) TransactionType.Spending)
                        .ToList());

            return GetSpreadingStatisticItems(transactionListFunc);
        }

        public static ObservableCollection<StatisticItem> GetSpreading() {
            if (allTransaction == null) {
                transactionData.LoadList();
            }

            if (allCategories == null) {
                cateogryData.LoadList();
            }

            var transactionListFunc =
                new Func<List<FinancialTransaction>>(() =>
                    allTransaction
                        .Where(x => x.Category != null)
                        .Where(x => x.Date.Month == DateTime.Today.Date.Month)
                        .Where(x => x.Type == (int) TransactionType.Spending)
                        .ToList());

            return GetSpreadingStatisticItems(transactionListFunc);
        }

        private static ObservableCollection<StatisticItem> GetSpreadingStatisticItems(
            Func<List<FinancialTransaction>> getTransactionListFunc) {
            List<FinancialTransaction> transactionList = getTransactionListFunc();

            List<StatisticItem> tempStatisticList = allCategories.Select(category => new StatisticItem {
                Category = category.Name,
                Value = transactionList
                    .Where(x => x.Category == category)
                    .Sum(x => x.Amount),
            }).ToList();

            RemoveNullList(tempStatisticList);

            tempStatisticList = tempStatisticList.OrderByDescending(x => x.Value).ToList();
            List<StatisticItem> statisticList = tempStatisticList.Take(6).ToList();

            AddOtherItem(tempStatisticList, statisticList);

            IncludeIncome(statisticList);

            return new ObservableCollection<StatisticItem>(statisticList);
        }

        private static void RemoveNullList(ICollection<StatisticItem> tempStatisticList) {
            List<StatisticItem> nullerList = tempStatisticList.Where(x => x.Value == 0).ToList();
            foreach (StatisticItem statisticItem in nullerList) {
                tempStatisticList.Remove(statisticItem);
            }
        }

        private static void SetLabel(StatisticItem item) {
            item.Label = item.Category + ": " + item.Value + " " + settings.DefaultCurrency;
        }

        private static void IncludeIncome(IEnumerable<StatisticItem> statisticList) {
            foreach (StatisticItem statisticItem in statisticList) {
                statisticItem.Value -= allTransaction
                    .Where(x => x.Type == (int) TransactionType.Income)
                    .Where(x => x.Category != null)
                    .Where(x => x.Category.Name == statisticItem.Category)
                    .Sum(x => x.Amount);

                SetLabel(statisticItem);

                if (statisticItem.Value <= 0) {
                    statisticItem.Value = 0;
                }
            }
        }

        private static void AddOtherItem(IEnumerable<StatisticItem> tempStatisticList,
            ICollection<StatisticItem> statisticList) {
            if (statisticList.Count < 6) return;

            var othersItem = new StatisticItem {
                Category = "Others",
                Value = tempStatisticList
                    .Where(x => !statisticList.Contains(x))
                    .Sum(x => x.Value)
            };
            othersItem.Label = othersItem.Category + ": " + othersItem.Value + " " + settings.DefaultCurrency;

            if (othersItem.Value > 0) {
                statisticList.Add(othersItem);
            }
        }
    }
}