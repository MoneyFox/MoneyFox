using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.StatisticDataProvider;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;

namespace MoneyFox.BusinessLogic.StatisticDataProvider
{
    public interface ICategorySummaryDataProvider
    {
        Task<IEnumerable<CategoryOverviewItem>> GetValues(DateTime startDate, DateTime endDate);
    }

    public class CategorySummaryDataProvider : ICategorySummaryDataProvider
    {
        private readonly IStatisticDbAccess statisticDbAccess;

        private List<Category> categories = new List<Category>();

        public CategorySummaryDataProvider(IStatisticDbAccess statisticDbAccess)
        {
            this.statisticDbAccess = statisticDbAccess;
        }

        public async Task<IEnumerable<CategoryOverviewItem>> GetValues(DateTime startDate, DateTime endDate)
        {
            var categoryOverviewItems = new List<CategoryOverviewItem>();
            categories = await statisticDbAccess.GetAllCategoriesWithPayments()
                                                ;

            foreach (Category category in categories)
            {
                if (category.Payments == null) continue;

                var categoryOverViewItem = new CategoryOverviewItem
                {
                    Label = category.Name,
                    Value = category.Payments
                                    .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date)
                                    .Where(x => x.Type != PaymentType.Transfer)
                                    .Sum(x => x.Type == PaymentType.Expense
                                             ? -x.Amount
                                             : x.Amount),
                    Average = CalculateAverage(category.Id)
                };
                categoryOverviewItems.Add(categoryOverViewItem);
            }

            CalculatePercentage(categoryOverviewItems);

            StatisticUtilities.RoundStatisticItems(categoryOverviewItems);

            return categoryOverviewItems.Where(x => Math.Abs(x.Value) > 0.1)
                             .OrderBy(x => x.Value)
                             .ToList();
        }

        private static void CalculatePercentage(List<CategoryOverviewItem> categories)
        {
            double sumNegative = categories.Where(x => x.Value < 0).Sum(x => x.Value);
            double sumPositive = categories.Where(x => x.Value > 0).Sum(x => x.Value);

            foreach (CategoryOverviewItem statisticItem in categories.Where(x => x.Value < 0))
            {
                statisticItem.Percentage = statisticItem.Value / sumNegative * 100;
            }

            foreach (CategoryOverviewItem statisticItem in categories.Where(x => x.Value > 0))
            {
                statisticItem.Percentage = statisticItem.Value / sumPositive * 100;
            }
        }

        private double CalculateAverage(int id)
        {
            List<Payment> payments = categories
                                     .First(x => x.Id == id)
                                     .Payments
                                     .Where(x => x.Date.Date > DateTime.Today.AddYears(-1))
                                     .OrderByDescending(x => x.Date)
                                     .ToList();

            if (payments.Count == 0) return 0;

            double sumForCategory = payments.Sum(x => x.Amount);
            TimeSpan timeDiff = DateTime.Today - DateTime.Today.AddYears(-1);

            if(timeDiff.Days < 30) return sumForCategory;
            return Math.Round(sumForCategory / (timeDiff.Days / 30), 2, MidpointRounding.ToEven);
        }
    }
}
