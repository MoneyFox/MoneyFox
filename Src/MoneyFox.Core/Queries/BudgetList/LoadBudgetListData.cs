namespace MoneyFox.Core.Queries.BudgetList;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core.Common;
using MoneyFox.Core.Common.Extensions;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Domain.Aggregates.AccountAggregate;

public static class LoadBudgetListData
{
    public class Query : IRequest<IReadOnlyCollection<BudgetListData>> { }

    public class Handler : IRequestHandler<Query, IReadOnlyCollection<BudgetListData>>
    {
        private readonly IAppDbContext appDbContext;
        private readonly ISystemDateHelper systemDateHelper;

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
                var thresholdDate = systemDateHelper.Today.GetFirstDayOfMonth().AddMonths(-(budget.Interval.NumberOfMonths - 1));
                var payments = await appDbContext.Payments.Where(p => p.Type != PaymentType.Transfer)
                    .Where(p => p.CategoryId != null)
                    .Where(p => p.Date >= thresholdDate)
                    .Where(p => budget.IncludedCategories.Contains(p.CategoryId!.Value))
                    .OrderBy(p => p.Date)
                    .ToListAsync(cancellationToken);

                if (payments.Any() is false)
                {
                    budgetListDataList.Add(new(id: budget.Id.Value, name: budget.Name, spendingLimit: budget.SpendingLimit, currentSpending: 0));

                    continue;
                }

                // Since sum is not supported for decimal in Ef Core with SQLite we have to do this in two steps
                var currentSpending = payments.Sum(selector: p => p.Type == PaymentType.Expense ? p.Amount : -p.Amount);
                if (currentSpending < 0)
                {
                    currentSpending = 0;
                }

                budgetListDataList.Add(new(id: budget.Id.Value, name: budget.Name, spendingLimit: budget.SpendingLimit, currentSpending: currentSpending));
            }

            return budgetListDataList;
        }
    }
}
