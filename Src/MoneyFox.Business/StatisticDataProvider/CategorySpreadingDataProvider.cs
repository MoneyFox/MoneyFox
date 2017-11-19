using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Models;

namespace MoneyFox.Business.StatisticDataProvider
{
    public class CategorySpreadingDataProvider
    {
        private readonly IPaymentService paymentService;

        public CategorySpreadingDataProvider(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        /// <summary>
        ///     Selects payments from the given timeframe and calculates the spreading for the six categories
        ///     with the highest spendings. All others are summarized in a "other" item.
        /// </summary>
        /// <param name="startDate">Startpoint form which to select data.</param>
        /// <param name="endDate">Endpoint form which to select data.</param>
        /// <returns>Statistic value for the given time. </returns>
        public async Task<IEnumerable<StatisticItem>> GetValues(DateTime startDate, DateTime endDate)
        {
            var paymentEnumerable = await paymentService.GetPaymentsWithoutTransfer(startDate, endDate);
            return GetSpreadingStatisticItems(paymentEnumerable.ToList());
        }

        private IEnumerable<StatisticItem> GetSpreadingStatisticItems(List<Payment> payments)
        {
            var tempStatisticList = (from payment in payments
                    group payment by new
                    {
                        category = payment.Data.Category != null ? payment.Data.Category.Name : string.Empty
                    }
                    into temp
                    select new StatisticItem
                    {
                        Label = temp.Key.category,
                        // we subtract income payments here so that we have all expenses without presign
                        Value = temp.Sum(x => x.Data.Type == PaymentType.Income ? -x.Data.Amount : x.Data.Amount)
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
                statisticItem.Label = statisticItem.Label
                                      + ": "
                                      + statisticItem.Value.ToString("C")
                                      + " ("
                                      + Math.Round(statisticItem.Value/totAmount*100, 2)
                                      + "%)";
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
                Label = "Others",
                Value = tempStatisticList
                    .Where(x => !statisticList.Contains(x))
                    .Sum(x => x.Value)
            };

            othersItem.Label = othersItem.Label + ": " + othersItem.Value;

            if (othersItem.Value > 0)
            {
                statisticList.Add(othersItem);
            }
        }
    }
}