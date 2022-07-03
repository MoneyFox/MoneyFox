namespace MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Domain.Aggregates.AccountAggregate;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public static class LoadBudgetListData
    {
        public class Query : IRequest<IReadOnlyCollection<BudgetListData>> { }

        public class Handler : IRequestHandler<Query, IReadOnlyCollection<BudgetListData>>
        {
            private readonly IAppDbContext appDbContext;

            public Handler(IAppDbContext appDbContext)
            {
                this.appDbContext = appDbContext;
            }

            public async Task<IReadOnlyCollection<BudgetListData>> Handle(Query request, CancellationToken cancellationToken)
            {
                var budgets = await appDbContext.Budgets.ToListAsync(cancellationToken);
                var budgetListDataList = new List<BudgetListData>();
                foreach (var budget in budgets)
                {
                    var payments = await appDbContext.Payments.Where(p => p.Type != PaymentType.Transfer)
                        .Where(p => p.CategoryId != null)
                        .Where(p => p.Date >= DateTime.Today.AddYears(-3))
                        .Where(p => budget.IncludedCategories.Contains(p.CategoryId!.Value))
                        .OrderBy(p => p.Date)
                        .ToListAsync(cancellationToken);

                    if (payments.Any() is false)
                    {
                        budgetListDataList.Add(new BudgetListData(id: budget.Id, name: budget.Name, spendingLimit: budget.SpendingLimit, currentSpending: 0));

                        continue;
                    }

                    var timeDeltaFirstPaymentAndNow = DateTime.Now - payments.First().Date;
                    var numberOfMonthsInRange = (int)Math.Ceiling(timeDeltaFirstPaymentAndNow.TotalDays / 30);

                    // Since sum is not supported for decimal in Ef Core with SQLite we have to do this in two steps
                    var currentSpending = payments.Sum(selector: p => p.Type == PaymentType.Expense ? p.Amount : -p.Amount);
                    var monthlyAverage = currentSpending / numberOfMonthsInRange;
                    budgetListDataList.Add(
                        new BudgetListData(id: budget.Id, name: budget.Name, spendingLimit: budget.SpendingLimit, currentSpending: monthlyAverage));
                }

                return budgetListDataList;
            }
        }
    }

}
