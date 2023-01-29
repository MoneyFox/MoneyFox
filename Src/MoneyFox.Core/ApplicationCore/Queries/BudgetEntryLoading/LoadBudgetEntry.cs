namespace MoneyFox.Core.ApplicationCore.Queries.BudgetEntryLoading;

using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.BudgetAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class LoadBudgetEntry
{
    public class Query : IRequest<BudgetEntryData>
    {
        public Query(int budgetId)
        {
            BudgetId = budgetId;
        }

        public int BudgetId { get; }
    }

    public class Handler : IRequestHandler<Query, BudgetEntryData>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<BudgetEntryData> Handle(Query request, CancellationToken cancellationToken)
        {
            var budgetData = await appDbContext.Budgets.Where(b => b.Id == new BudgetId(request.BudgetId))
                .Select(
                    b => new
                    {
                        b.Id,
                        b.Name,
                        b.BudgetTimeRange,
                        b.SpendingLimit,
                        b.IncludedCategories
                    })
                .AsNoTracking()
                .SingleAsync(cancellationToken);

            var budgetEntryCategories = appDbContext.Categories.Where(c => budgetData.IncludedCategories.Contains(c.Id))
                .Select(c => new BudgetEntryData.BudgetCategory(c.Id, c.Name))
                .AsNoTracking()
                .ToImmutableList();

            return new(
                id: budgetData.Id,
                name: budgetData.Name,
                spendingLimit: budgetData.SpendingLimit,
                timeRange: budgetData.BudgetTimeRange,
                categories: budgetEntryCategories);
        }
    }
}
