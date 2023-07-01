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

    public class Query : IRequest<Data>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
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
                .HasDateLargerEqualsThan(request.StartDate.Date)
                .HasDateSmallerEqualsThan(request.EndDate.Date)
                .ToListAsync(cancellationToken);

            var incomeAmount = payments.Where(x => x.Type == PaymentType.Income).Sum(x => x.Amount);
            var expenseAmount = payments.Where(x => x.Type == PaymentType.Expense).Sum(x => x.Amount);
            var valueIncreased = incomeAmount - expenseAmount;

            return new(Income: incomeAmount, Expense: expenseAmount, Gain: valueIncreased);
        }
    }
}
