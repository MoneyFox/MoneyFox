namespace MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading
{

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
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
                return await contextAdapter.Context.Budgets.Select(b => new BudgetListData(b.Id, b.Name, b.SpendingLimit)).ToListAsync(cancellationToken);
            }
        }
    }

}
