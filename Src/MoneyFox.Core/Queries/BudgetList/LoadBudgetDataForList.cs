namespace MoneyFox.Core.Queries.BudgetList;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Extensions;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class LoadBudgetDataForList
{
    public class Query : IRequest<IReadOnlyCollection<BudgetData>>;

    public class Handler : IRequestHandler<Query, IReadOnlyCollection<BudgetData>>
    {
        private readonly IAppDbContext appDbContext;
        private readonly ISystemDateHelper systemDateHelper;

        public Handler(ISystemDateHelper systemDateHelper, IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.systemDateHelper = systemDateHelper;
        }

        public async Task<IReadOnlyCollection<BudgetData>> Handle(Query request, CancellationToken cancellationToken)
        {
            var budgets = await appDbContext.Budgets.ToListAsync(cancellationToken);
            var budgetListDataList = new List<BudgetData>();
            foreach (var budget in budgets)
            {
                var monthlyBudget = budget.SpendingLimit / budget.Interval;
                var thresholdDate = systemDateHelper.Today.GetFirstDayOfMonth().AddMonths(-(budget.Interval.NumberOfMonths - 1));
                var payments = await appDbContext.Payments.Where(p => p.Type != PaymentType.Transfer)
                    .Where(p => p.CategoryId != null)
                    .Where(p => p.Date >= thresholdDate)
                    .Where(p => budget.IncludedCategories.Contains(p.CategoryId!.Value))
                    .OrderBy(p => p.Date)
                    .ToListAsync(cancellationToken);

                if (payments.Any() is false)
                {
                    budgetListDataList.Add(
                        new(
                            Id: budget.Id.Value,
                            Name: budget.Name,
                            SpendingLimit: budget.SpendingLimit,
                            CurrentSpending: 0,
                            MonthlyBudget: monthlyBudget,
                            MonthlySpending: 0));

                    continue;
                }

                // Since sum is not supported for decimal in Ef Core with SQLite we have to do this in two steps
                var currentSpending = payments.Sum(selector: p => p.Type == PaymentType.Expense ? p.Amount : -p.Amount);
                if (currentSpending < 0)
                {
                    currentSpending = 0;
                }

                var monthlySpending = currentSpending / budget.Interval;
                budgetListDataList.Add(
                    new(
                        Id: budget.Id.Value,
                        Name: budget.Name,
                        SpendingLimit: budget.SpendingLimit,
                        CurrentSpending: currentSpending,
                        MonthlyBudget: monthlyBudget,
                        MonthlySpending: monthlySpending));
            }

            return budgetListDataList;
        }
    }
}
