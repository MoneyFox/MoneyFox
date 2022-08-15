namespace MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Helpers;
    using Common.Interfaces;
    using Domain.Aggregates.AccountAggregate;
    using Domain.Aggregates.BudgetAggregate;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public static class LoadBudgetListData
    {
        public class Query : IRequest<IReadOnlyCollection<BudgetListData>> { }

        public class Handler : IRequestHandler<Query, IReadOnlyCollection<BudgetListData>>
        {
            private readonly ISystemDateHelper systemDateHelper;
            private readonly IAppDbContext appDbContext;

            public Handler(ISystemDateHelper systemDateHelper, IAppDbContext appDbContext)
            {
                this.appDbContext = appDbContext;
                this.systemDateHelper = systemDateHelper;
            }

            public async Task<IReadOnlyCollection<BudgetListData>> Handle(Query request, CancellationToken cancellationToken)
            {
                var budgets = await appDbContext.Budgets.ToListAsync(cancellationToken);
                var budgetListDataList = new List<BudgetListData>();
                foreach (var budget in budgets)
                {
                    var thresholdDate = GetThresholdDateFor(budget.BudgetTimeRange);
                    var payments = await appDbContext.Payments.Where(p => p.Type != PaymentType.Transfer)
                        .Where(p => p.CategoryId != null)
                        .Where(p => p.Date >= thresholdDate)
                        .Where(p => budget.IncludedCategories.Contains(p.CategoryId!.Value))
                        .OrderBy(p => p.Date)
                        .ToListAsync(cancellationToken);

                    if (payments.Any() is false)
                    {
                        budgetListDataList.Add(new BudgetListData(id: budget.Id, name: budget.Name, spendingLimit: budget.SpendingLimit, currentSpending: 0));

                        continue;
                    }

                    var timeDeltaFirstPaymentAndNow = systemDateHelper.Now.Date - thresholdDate.Date;
                    var numberOfMonthsInRange = (int)Math.Floor(timeDeltaFirstPaymentAndNow.TotalDays / 30);

                    // Since sum is not supported for decimal in Ef Core with SQLite we have to do this in two steps
                    var currentSpending = payments.Sum(selector: p => p.Type == PaymentType.Expense ? p.Amount : -p.Amount);
                    var monthlyAverage = currentSpending / numberOfMonthsInRange;
                    budgetListDataList.Add(
                        new BudgetListData(id: budget.Id, name: budget.Name, spendingLimit: budget.SpendingLimit, currentSpending: monthlyAverage));
                }

                return budgetListDataList;
            }

            private DateTime GetThresholdDateFor(BudgetTimeRange timeRange)
            {
                return timeRange switch
                {
                    BudgetTimeRange.YearToDate => new DateTime(systemDateHelper.Today.Year, 1, 1),
                    BudgetTimeRange.Last1Year => systemDateHelper.Today.AddYears(-1),
                    BudgetTimeRange.Last2Years => systemDateHelper.Today.AddYears(-2),
                    BudgetTimeRange.Last3Years => systemDateHelper.Today.AddYears(-3),
                    BudgetTimeRange.Last5Years => systemDateHelper.Today.AddYears(-5),
                    _ => throw new ArgumentOutOfRangeException(nameof(timeRange), timeRange, null)
                };
            }
        }
    }

}
