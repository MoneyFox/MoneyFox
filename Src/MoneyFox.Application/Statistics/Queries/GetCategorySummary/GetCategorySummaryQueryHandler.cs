using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.BusinessDbAccess.QueryObjects;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Statistics.Queries.GetCategorySummary
{
    public class GetCategorySummaryQueryHandler : IRequestHandler<GetCategorySummaryQuery, List<CategoryOverviewItem>>
    {
        private readonly IEfCoreContext context;

        public GetCategorySummaryQueryHandler(IEfCoreContext context)
        {
            this.context = context;
        }

        private List<Category> categories;

        public async Task<List<CategoryOverviewItem>> Handle(GetCategorySummaryQuery request, CancellationToken cancellationToken)
        {
            var categoryOverviewItems = new List<CategoryOverviewItem>();
            categories = await GetCategories(request, cancellationToken);

            foreach (Category category in categories)
            {
                if (category.Payments == null) continue;

                var categoryOverViewItem = new CategoryOverviewItem
                {
                    Label = category.Name,
                    Value = category.Payments
                                    .Where(x => x.Date.Date >= request.StartDate.Date && x.Date.Date <= request.EndDate.Date)
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

        private async Task<List<Category>> GetCategories(GetCategorySummaryQuery request, CancellationToken token) => await context.Categories
                                                                                                          .Include(x => x.Payments)
                                                                                                          .OrderByName()
                                                                                                          .ToListAsync(token);


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

            if (timeDiff.Days < 30) return sumForCategory;
            return Math.Round(sumForCategory / (timeDiff.Days / 30), 2, MidpointRounding.ToEven);
        }
    }
}
