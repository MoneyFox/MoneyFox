using System;
using System.Collections.Generic;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.StatisticDataProvider
{
    public class CategorySpreadingDataProvider : IStatisticProvider<IEnumerable<StatisticItem>>
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IPaymentRepository paymentRepository;

        public CategorySpreadingDataProvider(IPaymentRepository paymentRepository,
            IRepository<Category> categoryRepository)
        {
            this.paymentRepository = paymentRepository;
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        ///     Selects payments from the given timeframe and calculates the spreading for the six categories
        ///     with the highest spendings. All others are summarized in a "other" item.
        /// </summary>
        /// <param name="startDate">Startpoint form which to select data.</param>
        /// <param name="endDate">Endpoint form which to select data.</param>
        /// <returns>Statistic value for the given time. </returns>
        public IEnumerable<StatisticItem> GetValues(DateTime startDate, DateTime endDate)
        {
            // Get all Payments inlcuding income.
            var getPaymentListFunc =
                new Func<List<Payment>>(() =>
                    paymentRepository.Data
                        .Where(x => x.Category != null)
                        .Where(x => x.Date >= startDate.Date && x.Date <= endDate.Date)
                        .Where(x => x.Type == (int) PaymentType.Expense || x.Type == (int) PaymentType.Income)
                        .ToList());

            return GetSpreadingStatisticItems(getPaymentListFunc);
        }

        private List<StatisticItem> GetSpreadingStatisticItems(
            Func<List<Payment>> getPaymentListFunc)
        {
            var payments = getPaymentListFunc();

            var tempStatisticList = categoryRepository.Data.Select(category => new StatisticItem
            {
                Category = category.Name,
                Value = payments
                    .Where(x => x.Type == (int) PaymentType.Expense)
                    .Where(x => x.Category.Id == category.Id)
                    .Sum(x => x.Amount)
            }).ToList();

            RemoveZeroAmountEntries(tempStatisticList);

            tempStatisticList = tempStatisticList.OrderByDescending(x => x.Value).ToList();
            var statisticList = tempStatisticList.Take(6).ToList();

            AddOtherItem(tempStatisticList, statisticList);

            IncludeIncome(statisticList, payments);

            // Remove again all entries with zero amount.
            RemoveZeroAmountEntries(statisticList);

            SetLabel(statisticList);

            return statisticList;
        }

        private static void SetLabel(List<StatisticItem> statisticList)
        {
            var totAmount = statisticList.Sum(x => x.Value);
            foreach (var statisticItem in statisticList)
            {
                statisticItem.Label = statisticItem.Category 
                    + ": "
                    + statisticItem.Value.ToString("C") 
                    + " ("
                    + Math.Round(statisticItem.Value/totAmount*100, 2)
                    + "%)";
            }
        }

        private void RemoveZeroAmountEntries(ICollection<StatisticItem> tempStatisticList)
        {
            var nullerList = tempStatisticList.Where(x => Math.Abs(x.Value) < 0.001).ToList();
            foreach (var statisticItem in nullerList)
            {
                tempStatisticList.Remove(statisticItem);
            }
        }

        private void SetLabel(StatisticItem item)
        {
            item.Label = item.Category;
        }

        private void IncludeIncome(IEnumerable<StatisticItem> statisticList, List<Payment> payments)
        {
            foreach (var statisticItem in statisticList)
            {
                statisticItem.Value -= payments
                    .Where(x => x.Type == (int) PaymentType.Income)
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
    }
}