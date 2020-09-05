using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Statistics.Queries.GetCategorySummary
{
    public class GetCategorySummaryQuery : IRequest<CategorySummaryModel>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public class GetCategorySummaryQueryHandler : IRequestHandler<GetCategorySummaryQuery, CategorySummaryModel>
        {
            private const int PERCENTAGE_DIVIDER = 100;
            private const int DAY_DIVIDER = 30;
            private const int NUMBERS_OF_MONTHS_TO_LOAD = -12;
            private const int DECIMAL_DELTA = 0.1m;

            private readonly IContextAdapter contextAdapter;

            public GetCategorySummaryQueryHandler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            private List<Payment> paymentLastTwelveMonths = new List<Payment>();
            private List<CategoryOverviewItem> categoryOverviewItems = new List<CategoryOverviewItem>();

            public async Task<CategorySummaryModel> Handle(GetCategorySummaryQuery request, CancellationToken cancellationToken)
            {
                categoryOverviewItems = new List<CategoryOverviewItem>();

                paymentLastTwelveMonths = await contextAdapter.Context
                                                              .Payments
                                                              .Include(x => x.Category)
                                                              .Where(x => x.Date.Date >= DateTime.Today.AddMonths(NUMBERS_OF_MONTHS_TO_LOAD))
                                                              .WithoutTransfers()
                                                              .ToListAsync(cancellationToken);

                List<Payment> paymentsInTimeRange = await contextAdapter.Context
                                                                        .Payments
                                                                        .Include(x => x.Category)
                                                                        .HasDateLargerEqualsThan(request.StartDate.Date)
                                                                        .HasDateSmallerEqualsThan(request.EndDate.Date)
                                                                        .Where(x => x.Type != PaymentType.Transfer)
                                                                        .ToListAsync(cancellationToken);

                foreach(Category category in paymentsInTimeRange.Where(x => x.Category != null).Select(x => x.Category!).Distinct())
                {
                    CreateOverviewItem(paymentsInTimeRange, category);
                }

                AddEntryForPaymentsWithoutCategory(paymentsInTimeRange);

                CalculatePercentage(categoryOverviewItems);
                StatisticUtilities.RoundStatisticItems(categoryOverviewItems);

                return new CategorySummaryModel(Convert.ToDecimal(paymentsInTimeRange.Where(x => x.Type == PaymentType.Income).Sum(x => x.Amount)),
                                                Convert.ToDecimal(paymentsInTimeRange.Where(x => x.Type == PaymentType.Expense).Sum(x => x.Amount)),
                                                categoryOverviewItems.Where(x => Math.Abs(x.Value) > DECIMAL_DELTA).OrderBy(x => x.Value).ToList());
            }

            private void CreateOverviewItem(IEnumerable<Payment> payments, Category category)
            {
                var categoryOverViewItem = new CategoryOverviewItem
                {
                    CategoryId = category.Id,
                    Label = category.Name,
                    Value = payments.Where(x => x.Category != null)
                                    .Where(x => x.Category!.Id == category.Id)
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
                decimal sumNegative = categories.Where(x => x.Value < 0).Sum(x => x.Value);
                decimal sumPositive = categories.Where(x => x.Value > 0).Sum(x => x.Value);

                foreach(CategoryOverviewItem statisticItem in categories.Where(x => x.Value < 0))
                {
                    statisticItem.Percentage = statisticItem.Value / sumNegative * PERCENTAGE_DIVIDER;
                }

                foreach(CategoryOverviewItem statisticItem in categories.Where(x => x.Value > 0))
                {
                    statisticItem.Percentage = statisticItem.Value / sumPositive * PERCENTAGE_DIVIDER;
                }
            }

            private decimal CalculateAverageForCategory(int id)
            {
                List<Payment> payments = paymentLastTwelveMonths
                                        .Where(x => x.Category != null)
                                        .Where(x => x.Category!.Id == id)
                                        .OrderByDescending(x => x.Date)
                                        .ToList();

                if(payments.Count == 0)
                    return 0;

                return SumForCategory(payments);
            }

            private decimal CalculateAverageForPaymentsWithoutCategory()
            {
                List<Payment> payments = paymentLastTwelveMonths
                                        .Where(x => x.Category == null)
                                        .OrderByDescending(x => x.Date)
                                        .ToList();

                if(payments.Count == 0)
                    return 0;

                return SumForCategory(payments);
            }

            private static decimal SumForCategory(IEnumerable<Payment> payments)
            {
                decimal sumForCategory = payments.Sum(x => x.Amount);
                TimeSpan timeDiff = DateTime.Today - DateTime.Today.AddYears(-1);

                if(timeDiff.Days < DAY_DIVIDER)
                    return sumForCategory;

                return Math.Round(sumForCategory / (timeDiff.Days / DAY_DIVIDER), 2, MidpointRounding.ToEven);
            }
        }
    }
}
