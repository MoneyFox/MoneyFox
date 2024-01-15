namespace MoneyFox.Core.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Extensions;
using Common.Interfaces;
using Domain.Aggregates.BudgetAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetPaymentsInBudget
{
    internal record Data(
        int PaymentId,
        decimal Amount,
        string Account,
        string Category,
        bool IsCleared,
        bool IsRecurring);

    public record Query(BudgetId BudgetId) : IRequest<IReadOnlyList<Data>>;

    public class Handler(IAppDbContext dbContext, ISystemDateHelper systemDateHelper) : IRequestHandler<Query, IReadOnlyList<Data>>
    {
        public async Task<IReadOnlyList<Data>> Handle(Query query, CancellationToken cancellationToken)
        {
            var budget = await dbContext.Budgets.Where(b => b.Id == query.BudgetId).SingleAsync(cancellationToken);

            var thresholdDate = systemDateHelper.Today.GetFirstDayOfMonth().AddMonths(-(budget.Interval.NumberOfMonths - 1));
            var endOfMonth = systemDateHelper.Today.GetLastDayOfMonth();
            return await dbContext.Payments
                .Where(p => p.Category != null && budget.IncludedCategories.Contains(p.Category!.Id))
                .Where(p => p.Date >= thresholdDate)
                .Where(p => p.Date <= endOfMonth)
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
