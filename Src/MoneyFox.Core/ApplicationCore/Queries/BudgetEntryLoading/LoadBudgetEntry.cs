namespace MoneyFox.Core.ApplicationCore.Queries.BudgetEntryLoading
{

    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
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
                var emptyList = ImmutableList<BudgetEntryData.BudgetCategory>.Empty;

                return await appDbContext.Budgets.Where(b => b.Id == request.BudgetId)
                                         .Select(b => new BudgetEntryData(b.Id, b.Name, b.SpendingLimit, emptyList))
                                         .AsNoTracking()
                                         .SingleAsync(cancellationToken);
            }
        }
    }

}
