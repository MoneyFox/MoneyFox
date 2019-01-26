using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.StatisticDataProvider;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Models;

namespace MoneyFox.BusinessLogic.StatisticDataProvider
{
    public interface ICategorySummaryDataProvider
    {
        Task<IEnumerable<StatisticItem>> GetValues(DateTime startDate, DateTime endDate);
    }

    public class CategorySummaryDataProvider : ICategorySummaryDataProvider
    {
        private readonly IStatisticDbAccess statisticDbAccess;

        public CategorySummaryDataProvider(IStatisticDbAccess statisticDbAccess)
        {
            this.statisticDbAccess = statisticDbAccess;
        }

        public async Task<IEnumerable<StatisticItem>> GetValues(DateTime startDate, DateTime endDate)
        {
            var categories = new List<StatisticItem>();

            foreach (var category in await statisticDbAccess.GetAllCategoriesWithPayments())
            {
                if (category.Payments == null) continue;

                categories.Add(new StatisticItem
                {
                    Label = category.Name,
                    Value = category.Payments
                        .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date)
                        .Where(x => x.Type != PaymentType.Transfer)
                        .Sum(x => x.Type == PaymentType.Expense
                            ? -x.Amount
                            : x.Amount)
                });
            }

            CalculateAverage(categories);
            Utilities.RoundStatisticItems(categories);

            return categories.Where(x => Math.Abs(x.Value) > 0.1)
                             .OrderBy(x => x.Value)
                             .ToList();
        }

        private static void CalculateAverage(List<StatisticItem> categories)
        {
            var sumNegative = categories.Where(x => x.Value < 0).Sum(x => x.Value);
            var sumPositive = categories.Where(x => x.Value > 0).Sum(x => x.Value);

            foreach (var statisticItem in categories.Where(x => x.Value < 0))
            {
                statisticItem.Percentage = statisticItem.Value / sumNegative * 100;
            }

            foreach (var statisticItem in categories.Where(x => x.Value > 0))
            {
                statisticItem.Percentage = statisticItem.Value / sumPositive * 100;
            }
        }
    }
}