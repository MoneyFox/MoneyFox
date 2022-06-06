namespace MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Interfaces;
    using Domain.Aggregates.AccountAggregate;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public static class LoadBudgets
    {
        public class Query : IRequest<IReadOnlyCollection<BudgetListData>> { }

        public class Handler : IRequestHandler<Query, IReadOnlyCollection<BudgetListData>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<IReadOnlyCollection<BudgetListData>> Handle(Query request, CancellationToken cancellationToken)
            {
                var dbContext = contextAdapter.Context;
                var budgets = await dbContext.Budgets.ToListAsync(cancellationToken);
                var budgetListDataList = new List<BudgetListData>();
                var firstDayOfCurrentMonth = DateTime.Now.GetFirstDayOfMonth();
                var lastDayOfCurrentMonth = DateTime.Now.GetLastDayOfMonth();
                foreach (var budget in budgets)
                {
                    var payments = await dbContext.Payments.Where(p => p.CategoryId != null)
                        .Where(p => p.Date >= firstDayOfCurrentMonth && p.Date <= lastDayOfCurrentMonth)
                        .Where(p => budget.IncludedCategories.Contains(p.CategoryId!.Value))
                        .ToListAsync(cancellationToken);

                    // Since sum is not supported for decimal in Ef Core with SQLite we have to do this in two steps
                    var currentSpending = payments.Sum(selector: p => p.Type == PaymentType.Expense ? p.Amount : -p.Amount);
                    budgetListDataList.Add(
                        new BudgetListData(id: budget.Id, name: budget.Name, spendingLimit: budget.SpendingLimit, currentSpending: currentSpending));
                }

                return budgetListDataList;
            }
        }
    }

}
