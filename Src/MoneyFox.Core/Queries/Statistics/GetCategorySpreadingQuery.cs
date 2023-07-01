namespace MoneyFox.Core.Queries.Statistics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetCategorySpreading
{
    public record DataSet(string CategoryName, decimal Value);

    public record Query : IRequest<IEnumerable<DataSet>>
    {
        private const int NUMBER_OF_STATISTIC_ITEMS = 10;

        public Query(
            DateOnly startDate,
            DateOnly endDate,
            PaymentType paymentType = PaymentType.Expense,
            int numberOfCategoriesToShow = NUMBER_OF_STATISTIC_ITEMS)
        {
            if (startDate > endDate)
            {
                throw new InvalidDateRangeException();
            }

            StartDate = startDate;
            EndDate = endDate;
            PaymentType = paymentType;
            NumberOfCategoriesToShow = numberOfCategoriesToShow;
        }

        public DateOnly StartDate { get; }

        public DateOnly EndDate { get; }

        public PaymentType PaymentType { get; }

        public int NumberOfCategoriesToShow { get; }
    }

    public class Handler : IRequestHandler<Query, IEnumerable<DataSet>>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IEnumerable<DataSet>> Handle(Query request, CancellationToken cancellationToken)
        {
            var paymentsWithoutTransferAsync = await GetPaymentsWithoutTransferAsync(request: request, cancellationToken: cancellationToken);

            return AggregateData(
                statisticData: SelectRelevantDataFromList(request: request, payments: paymentsWithoutTransferAsync),
                amountOfCategoriesToShow: request.NumberOfCategoriesToShow);
        }

        private async Task<IEnumerable<Payment>> GetPaymentsWithoutTransferAsync(Query request, CancellationToken cancellationToken)
        {
            return await appDbContext.Payments.Include(x => x.Category)
                .WithoutTransfers()
                .Where(payment => payment.Date.Date >= request.StartDate.ToDateTime(TimeOnly.MinValue))
                .Where(payment => payment.Date.Date <= request.EndDate.ToDateTime(TimeOnly.MinValue))
                .ToListAsync(cancellationToken);
        }

        private List<(decimal Value, string Label)> SelectRelevantDataFromList(Query request, IEnumerable<Payment> payments)
        {
            var query = from payment in payments
                group payment by new { category = payment.Category != null ? payment.Category.Name : string.Empty } into temp
                select (temp.Sum(x => x.Type == PaymentType.Income ? -x.Amount : x.Amount), temp.Key.category);

            query = request.PaymentType == PaymentType.Expense ? query.Where(x => x.Item1 > 0) : query.Where(x => x.Item1 < 0);

            return query.Select(x => (Math.Abs(x.Item1), x.category)).OrderByDescending(x => x.Item1).ToList();
        }

        private IEnumerable<DataSet> AggregateData(List<(decimal Value, string Label)> statisticData, int amountOfCategoriesToShow)
        {
            var statisticList = statisticData.Take(amountOfCategoriesToShow).Select(x => new DataSet(CategoryName: x.Label, Value: x.Value)).ToList();
            AddOtherItem(statisticData: statisticData, statisticList: statisticList, amountOfCategoriesToShow: amountOfCategoriesToShow);

            return statisticList;
        }

        private static void AddOtherItem(
            IEnumerable<(decimal Value, string Label)> statisticData,
            ICollection<DataSet> statisticList,
            int amountOfCategoriesToShow)
        {
            if (statisticList.Count < amountOfCategoriesToShow)
            {
                return;
            }

            var otherValue = statisticData.Where(x => statisticList.All(y => x.Label != y.CategoryName)).Sum(x => x.Value);
            var othersItem = new DataSet(CategoryName: "Other", Value: otherValue);
            if (othersItem.Value > 0)
            {
                statisticList.Add(othersItem);
            }
        }
    }
}
