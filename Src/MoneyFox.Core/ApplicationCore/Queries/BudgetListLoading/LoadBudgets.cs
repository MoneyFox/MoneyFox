namespace MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading
{

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
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
                foreach (var budget in budgets)
                {
                    var currentSpending = await dbContext.Payments.Where(p => p.CategoryId != null)
                        .Where(p => budget.IncludedCategories.Contains(p.CategoryId!.Value))
                        .SumAsync(selector: p => p.Type == PaymentType.Expense ? p.Amount : -p.Amount, cancellationToken: cancellationToken);

                    budgetListDataList.Add(
                        new BudgetListData(id: budget.Id, name: budget.Name, spendingLimit: budget.SpendingLimit, currentSpending: currentSpending));
                }

                return budgetListDataList;
            }
        }
    }

}
