namespace MoneyFox.Core.Queries.Accounts.GetIncludedAccountBalanceSummary
{
    using _Pending_.Common.Interfaces;
    using _Pending_.Common.QueryObjects;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetIncludedAccountBalanceSummaryQuery : IRequest<decimal>
    {
        public class Handler : IRequestHandler<GetIncludedAccountBalanceSummaryQuery, decimal>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<decimal> Handle(GetIncludedAccountBalanceSummaryQuery request,
                CancellationToken cancellationToken) =>
                (await contextAdapter.Context
                    .Accounts
                    .AreActive()
                    .AreNotExcluded()
                    .Select(x => x.CurrentBalance)
                    .ToListAsync(cancellationToken))
                .Sum();
        }
    }
}