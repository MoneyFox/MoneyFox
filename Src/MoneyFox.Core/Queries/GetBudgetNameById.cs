namespace MoneyFox.Core.Queries;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.BudgetAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetBudgetNameById
{
    public record Query(BudgetId BudgetId) : IRequest<string>;

    public class Handler(IAppDbContext dbContext) : IRequestHandler<Query, string>
    {
        public Task<string> Handle(Query query, CancellationToken cancellationToken)
        {
            return dbContext.Budgets.Where(b => b.Id == query.BudgetId).Select(b => b.Name).SingleAsync(cancellationToken);
        }
    }
}
