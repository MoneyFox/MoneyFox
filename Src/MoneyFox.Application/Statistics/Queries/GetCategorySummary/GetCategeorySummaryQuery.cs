using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Statistics.Queries.GetCategorySummary
{
    public class GetCategorySummaryQuery : IRequest<CategorySummaryModel>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public class GetCategorySummaryQueryHandler : IRequestHandler<GetCategorySummaryQuery, CategorySummaryModel>
        {
            private readonly IEfCoreContext context;

            public GetCategorySummaryQueryHandler(IEfCoreContext context)
            {
                this.context = context;
            }

            private List<Payment> paymentLastTwelveMonths;
            private List<CategoryOverviewItem> categoryOverviewItems;

            public async Task<CategorySummaryModel> Handle(GetCategorySummaryQuery request, CancellationToken cancellationToken)
            {
                categoryOverviewItems = new List<CategoryOverviewItem>();

                paymentLastTwelveMonths = await context.Payments
                                                       .Where(x => x.Date.Date >= DateTime.Today.AddMonths(-12))
                                                       .Where(x => x.Type != PaymentType.Transfer)
                                                       .ToListAsync(cancellationToken);

                List<Payment> paymentsInTimeRange = await context.Payments
                                                                 .Where(x => x.Date.Date >= request.StartDate.Date && x.Date.Date <= request.EndDate.Date)
                                                                 .Where(x => x.Type != PaymentType.Transfer)
                                                                 .ToListAsync(cancellationToken);


                foreach (var category in paymentsInTimeRange.Where(x => x.Category != null).Select(x => x.Category))
                {
                    CreateOverviewItem(paymentsInTimeRange, category);
                }

                AddEntryForPaymentsWithoutCategory(paymentsInTimeRange);

                CalculatePercentage(categoryOverviewItems);
                StatisticUtilities.RoundStatisticItems(categoryOverviewItems);

                return new CategorySummaryModel( Convert.ToDecimal(categoryOverviewItems.Where(x => x.Value < 0).Sum(x => x.Value)),
                                                 Convert.ToDecimal(categoryOverviewItems.Where(x => x.Value > 0).Sum(x => x.Value)),
                                                categoryOverviewItems.Where(x => Math.Abs(x.Value) > 0.1)
                                                                     .OrderBy(x => x.Value)
                                                                     .ToList());
            }

            private void CreateOverviewItem(List<Payment> payments, Category category)
            {
                var categoryOverViewItem = new CategoryOverviewItem
                {
                    Label = category.Name,
                    Value = payments.Where(x => x.Category.Id == category.Id)
                                    .Where(x => x.Type != PaymentType.Transfer)
                                    .Sum(x => x.Type == PaymentType.Expense
                                             ? -x.Amount
                                             : x.Amount),
                    Average = CalculateAverageForCategory(category.Id)
                };
                categoryOverviewItems.Add(categoryOverViewItem);
            }

            private void AddEntryForPaymentsWithoutCategory(List<Payment> payments)
            {
                categoryOverviewItems.Add(new CategoryOverviewItem
                {
                    Label = Strings.NoCategoryLabel,
                    Value = payments.Where(x => x.Category == null)
                                    .Where(x => x.Type != PaymentType.Transfer)
                                    .Sum(x => x.Type == PaymentType.Expense
                                             ? -x.Amount
                                             : x.Amount),
                    Average = CalculateAverageForPaymentsWithoutCategory()
                });
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

            private double CalculateAverageForCategory(int id)
            {
                List<Payment> payments = paymentLastTwelveMonths
                                         .Where(x => x.Category != null)
                                         .Where(x => x.Category.Id == id)
                                         .OrderByDescending(x => x.Date)
                                         .ToList();

                if (payments.Count == 0) return 0;

                return SumForCategory(payments);
            }

            private double CalculateAverageForPaymentsWithoutCategory()
            {
                List<Payment> payments = paymentLastTwelveMonths
                                         .Where(x => x.Category == null)
                                         .OrderByDescending(x => x.Date)
                                         .ToList();

                if (payments.Count == 0) return 0;

                return SumForCategory(payments);
            }

            private static double SumForCategory(List<Payment> payments)
            {
                double sumForCategory = payments.Sum(x => x.Amount);
                TimeSpan timeDiff = DateTime.Today - DateTime.Today.AddYears(-1);

                if (timeDiff.Days < 30) return sumForCategory;

                return Math.Round(sumForCategory / (timeDiff.Days / 30), 2, MidpointRounding.ToEven);
            }
        }
    }
}
