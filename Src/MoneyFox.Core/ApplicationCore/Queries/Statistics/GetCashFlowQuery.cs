namespace MoneyFox.Core.ApplicationCore.Queries.Statistics;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _Pending_.Common.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public record CashFlowData(decimal Income, decimal Expense, decimal Gain);

public class GetCashFlowQuery : IRequest<CashFlowData>
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}

public class GetCashFlowQueryHandler : IRequestHandler<GetCashFlowQuery, CashFlowData>
{
    private readonly IAppDbContext appDbContext;

    public GetCashFlowQueryHandler(IAppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public async Task<CashFlowData> Handle(GetCashFlowQuery request, CancellationToken cancellationToken)
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
