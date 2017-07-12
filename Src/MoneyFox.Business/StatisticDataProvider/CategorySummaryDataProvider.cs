using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Models;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Business.StatisticDataProvider
{
    public class CategorySummaryDataProvider
    {
        private readonly ICategoryService categoryService;

        public CategorySummaryDataProvider(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IEnumerable<StatisticItem>> GetValues(DateTime startDate, DateTime endDate)
        {
            var categories = new List<StatisticItem>();

            foreach (var category in await categoryService.GetAllCategoriesWithPayments())
            {
                if (category.Data.Payments == null) continue;

                categories.Add(new StatisticItem
                {
                    Label = category.Data.Name,
                    Value = category.Data.Payments
                        .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date)
                        .Where(x => x.Type != PaymentType.Transfer)
                        .Sum(x => x.Type == PaymentType.Expense
                            ? -x.Amount
                            : x.Amount)
                });
            }

            return categories.Where(x => Math.Abs(x.Value) > 0.1).OrderBy(x => x.Value).ToList();
        }
    }
}