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
            DateTime startDate,
            DateTime endDate,
            PaymentType paymentType = PaymentType.Expense,
            int numberOfCategoriesToShow = NUMBER_OF_STATISTIC_ITEMS)
        {
            StartDate = startDate;
            EndDate = endDate;
            PaymentType = paymentType;
            NumberOfCategoriesToShow = numberOfCategoriesToShow;
        }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public PaymentType PaymentType { get; }

        public int NumberOfCategoriesToShow { get; }
    }

    public class Handler : IRequestHandler<Query, IEnumerable<DataSet>>
    {
        private readonly IAppDbContext appDbContext;

        private Query currentRequest = null!;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IEnumerable<DataSet>> Handle(Query request, CancellationToken cancellationToken)
        {
            currentRequest = request;

            return AggregateData(
                statisticData: SelectRelevantDataFromList(await GetPaymentsWithoutTransferAsync(cancellationToken)),
                amountOfCategoriesToShow: request.NumberOfCategoriesToShow);
        }

        private async Task<IEnumerable<Payment>> GetPaymentsWithoutTransferAsync(CancellationToken cancellationToken)
        {
            return await appDbContext.Payments.Include(x => x.Category)
                .WithoutTransfers()
                .HasDateLargerEqualsThan(currentRequest.StartDate.Date)
                .HasDateSmallerEqualsThan(currentRequest.EndDate.Date)
                .ToListAsync(cancellationToken);
        }

        private List<(decimal Value, string Label)> SelectRelevantDataFromList(IEnumerable<Payment> payments)
        {
            var query = from payment in payments
                group payment by new { category = payment.Category != null ? payment.Category.Name : string.Empty } into temp
                select (temp.Sum(x => x.Type == PaymentType.Income ? -x.Amount : x.Amount), temp.Key.category);

            query = currentRequest.PaymentType == PaymentType.Expense ? query.Where(x => x.Item1 > 0) : query.Where(x => x.Item1 < 0);

            return query.Select(x => (Math.Abs(x.Item1), x.category)).OrderByDescending(x => x.Item1).ToList();
        }

        private IEnumerable<DataSet> AggregateData(List<(decimal Value, string Label)> statisticData, int amountOfCategoriesToShow)
        {
            var statisticList = statisticData.Take(amountOfCategoriesToShow)
                .Select(x => new DataSet(CategoryName: x.Label, Value: x.Value))
                .ToList();

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
