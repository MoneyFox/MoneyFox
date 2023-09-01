namespace MoneyFox.Core.Queries.Statistics;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetCashFlow
{
    public record Data(decimal Income, decimal Expense, decimal Gain);

    public record Query : IRequest<Data>
    {
        public Query(DateOnly startDate, DateOnly endDate)
        {
            if (startDate > endDate)
            {
                throw new InvalidDateRangeException();
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        public DateOnly StartDate { get; }
        public DateOnly EndDate { get; }
    }

    public class Handler : IRequestHandler<Query, Data>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Data> Handle(Query request, CancellationToken cancellationToken)
        {
            var payments = await appDbContext.Payments.Include(x => x.Category)
                .WithoutTransfers()
                .Where(payment => payment.Date.Date >= request.StartDate.ToDateTime(TimeOnly.MinValue))
                .Where(payment => payment.Date.Date <= request.EndDate.ToDateTime(TimeOnly.MinValue))
                .ToListAsync(cancellationToken);

            var incomeAmount = payments.Where(x => x.Type == PaymentType.Income).Sum(x => x.Amount);
            var expenseAmount = payments.Where(x => x.Type == PaymentType.Expense).Sum(x => x.Amount);
            var valueIncreased = incomeAmount - expenseAmount;

            return new(Income: incomeAmount, Expense: expenseAmount, Gain: valueIncreased);
        }
    }
}
