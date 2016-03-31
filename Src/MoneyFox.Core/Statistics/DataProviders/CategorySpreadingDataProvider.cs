using System;
using System.Collections.Generic;
using System.Linq;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Model;
using MoneyFox.Core.Statistics.Models;

namespace MoneyFox.Core.Statistics.DataProviders
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
        ///     Selects PaymentViewModels from the given timeframe and calculates the spreading for the six categories
        ///     with the highest spendings. All others are summarized in a "other" item.
        /// </summary>
        /// <param name="startDate">Startpoint form which to select data.</param>
        /// <param name="endDate">Endpoint form which to select data.</param>
        /// <returns>Statistic value for the given time. </returns>
        public IEnumerable<StatisticItem> GetValues(DateTime startDate, DateTime endDate)
        {
            // Get all PaymentViewModels inlcuding income.
            var getPaymentViewModelListFunc =
                new Func<List<PaymentViewModel>>(() =>
                    paymentRepository.Data
                        .Where(x => x.Category != null)
                        .Where(x => x.Date >= startDate.Date && x.Date <= endDate.Date)
                        .Where(x => x.Type == (int) PaymentType.Expense || x.Type == PaymentType.Income)
                        .ToList());

            return GetSpreadingStatisticItems(getPaymentViewModelListFunc);
        }

        private List<StatisticItem> GetSpreadingStatisticItems(
            Func<List<PaymentViewModel>> getPaymentViewModelListFunc)
        {
            var paymentViewModels = getPaymentViewModelListFunc();

            var tempStatisticList = categoryRepository.Data.Select(category => new StatisticItem
            {
                Category = category.Name,
                Value = paymentViewModels
                    .Where(x => x.Type == (int) PaymentType.Expense)
                    .Where(x => x.Category.Id == category.Id)
                    .Sum(x => x.Amount)
            }).ToList();

            RemoveZeroAmountEntries(tempStatisticList);

            tempStatisticList = tempStatisticList.OrderByDescending(x => x.Value).ToList();
            var statisticList = tempStatisticList.Take(6).ToList();

            AddOtherItem(tempStatisticList, statisticList);

            IncludeIncome(statisticList, paymentViewModels);

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

        private void IncludeIncome(IEnumerable<StatisticItem> statisticList, List<PaymentViewModel> PaymentViewModels)
        {
            foreach (var statisticItem in statisticList)
            {
                statisticItem.Value -= PaymentViewModels
                    .Where(x => x.Type == PaymentType.Income)
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