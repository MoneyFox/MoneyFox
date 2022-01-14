using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Statistics.Queries
{
    public class GetCategoryProgressionQuery : IRequest<IImmutableList<StatisticEntry>>
    {
        public GetCategoryProgressionQuery(int categoryId, DateTime startDate, DateTime endDate)
        {
            CategoryId = categoryId;
            StartDate = startDate;
            EndDate = endDate;

            if(startDate > EndDate)
            {
                throw new StartAfterEnddateException();
            }
        }

        public int CategoryId { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }
    }

    public class
        GetCategoryProgressionHandler : IRequestHandler<GetCategoryProgressionQuery, IImmutableList<StatisticEntry>>
    {
        private const string RED_HEX_CODE = "#cd3700";
        private const string BLUE_HEX_CODE = "#87cefa";

        private readonly IContextAdapter contextAdapter;

        public GetCategoryProgressionHandler(IContextAdapter contextAdapter)
        {
            this.contextAdapter = contextAdapter;
        }

        public async Task<IImmutableList<StatisticEntry>> Handle(GetCategoryProgressionQuery request,
            CancellationToken cancellationToken)
        {
            List<Payment>? payments = await contextAdapter.Context
                                                          .Payments
                                                          .Include(x => x.Category)
                                                          .Include(x => x.ChargedAccount)
                                                          .HasCategoryId(request.CategoryId)
                                                          .HasDateLargerEqualsThan(request.StartDate.Date)
                                                          .HasDateSmallerEqualsThan(request.EndDate.Date)
                                                          .ToListAsync(cancellationToken);

            var statisticList = new List<StatisticEntry>();
            foreach(var group in payments.GroupBy(x => new {x.Date.Month, x.Date.Year}))
            {
                var statisticEntry = new StatisticEntry(
                    group.Sum(x => GetPaymentAmountForSum(x, request)),
                    $"{group.Key.Month:d2} {group.Key.Year:d4}");
                statisticEntry.ValueLabel = statisticEntry.Value.ToString("c", CultureHelper.CurrentCulture);
                statisticEntry.Color = statisticEntry.Value >= 0 ? BLUE_HEX_CODE : RED_HEX_CODE;
                statisticList.Add(statisticEntry);
            }

            return FillReturnList(request, statisticList);
        }

        private static IImmutableList<StatisticEntry> FillReturnList(
            GetCategoryProgressionQuery request,
            IEnumerable<StatisticEntry> statisticEntries)
        {
            var returnList = new List<StatisticEntry>();
            DateTime startDate = request.StartDate;
            DateTime endDate = request.EndDate.AddMonths(1);

            do
            {
                List<StatisticEntry>? items = statisticEntries
                                              .Where(x => x.Label == $"{startDate.Month:d2} {startDate.Year:d4}")
                                              .ToList();

                returnList.AddRange(items);

                if(!items.Any())
                {
                    var placeholderItem = new StatisticEntry(0, $"{startDate.Month:d2} {startDate.Year:d4}");
                    placeholderItem.ValueLabel = placeholderItem.Value.ToString("c", CultureHelper.CurrentCulture);
                    placeholderItem.Color = placeholderItem.Value >= 0 ? BLUE_HEX_CODE : RED_HEX_CODE;

                    returnList.Add(placeholderItem);
                }

                startDate = startDate.AddMonths(1);
            } while(startDate.Month != endDate.Month || startDate.Year != endDate.Year);

            return returnList.ToImmutableList();
        }

        private static decimal GetPaymentAmountForSum(Payment payment, GetCategoryProgressionQuery request) =>
            payment.Type switch
            {
                PaymentType.Expense => -payment.Amount,
                PaymentType.Income => payment.Amount,
                PaymentType.Transfer => payment.ChargedAccount.Id == request.CategoryId
                    ? -payment.Amount
                    : payment.Amount,
                _ => 0
            };
    }
}