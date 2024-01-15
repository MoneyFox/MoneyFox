namespace MoneyFox.Core.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.BudgetAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

internal static class GetPaymentsInBudget
{
    internal record Data(
        int PaymentId,
        decimal Amount,
        string AccountName,
        string Category,
        bool IsCleared,
        bool IsRecurring);

    internal record Query(BudgetId BudgetId) : IRequest<IReadOnlyList<Data>>;

    public class Handler(IAppDbContext dbContext) : IRequestHandler<Query, IReadOnlyList<Data>>
    {
        public async Task<IReadOnlyList<Data>> Handle(Query query, CancellationToken cancellationToken)
        {
            var budgetCategories = await dbContext.Budgets.Where(b => b.Id == query.BudgetId).Select(b => b.IncludedCategories).SingleAsync(cancellationToken);

            var pi = dbContext.Payments.ToList();
            return await dbContext.Payments.Where(p => p.Category != null && budgetCategories.Contains(p.Category!.Id))
                .Select(
                    p => new Data(
                        p.Id,
                        p.Amount,
                        p.ChargedAccount.Name,
                        p.Category!.Name,
                        p.IsCleared,
                        p.IsRecurring))
                .ToListAsync(cancellationToken);
        }
    }
}
