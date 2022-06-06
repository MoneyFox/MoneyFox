namespace MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading
{

    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using MediatR;

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
                await Task.CompletedTask;
                return ImmutableList<BudgetListData>.Empty;
            }
        }
    }

}
