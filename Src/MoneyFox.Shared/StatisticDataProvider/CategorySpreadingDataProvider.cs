using System;
using System.Collections.Generic;
using System.Linq;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.StatisticDataProvider
{
    public class CategorySpreadingDataProvider : IStatisticProvider<IEnumerable<StatisticItem>>
    {
        private readonly IRepository<Payment> paymentRepository;

        public CategorySpreadingDataProvider(IRepository<Payment> paymentRepository)
        {
            this.paymentRepository = paymentRepository;
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
            return GetSpreadingStatisticItems(paymentRepository
                .GetList(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date
                              && (x.Type == (int) PaymentType.Expense || x.Type == (int) PaymentType.Income))
                .ToList());
        }

        private List<StatisticItem> GetSpreadingStatisticItems(List<Payment> payments)
        {
            var tempStatisticList = (from payment in payments
                group payment by new
                {
                    category = payment.Category != null ? payment.Category.Name : string.Empty
                }
                into temp
                select new StatisticItem
                {
                    Category = temp.Key.category,
                    // we subtract income payments here so that we have all expenses without presign
                    Value = temp.Sum(x => x.Type == (int) PaymentType.Income ? -x.Amount : x.Amount)
                })
                .Where(x => x.Value > 0)
                .OrderByDescending(x => x.Value)
                .ToList();

            var statisticList = tempStatisticList.Take(6).ToList();

            AddOtherItem(tempStatisticList, statisticList);
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