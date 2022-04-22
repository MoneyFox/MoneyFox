namespace MoneyFox.Core.ApplicationCore.Queries
{

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using MoneyFox.Core._Pending_.Common.QueryObjects;
    using MoneyFox.Core.Common.Interfaces;

    public class GetIncludedAccountBalanceSummaryQuery : IRequest<decimal>
    {
        public class Handler : IRequestHandler<GetIncludedAccountBalanceSummaryQuery, decimal>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<decimal> Handle(GetIncludedAccountBalanceSummaryQuery request, CancellationToken cancellationToken)
            {
                return (await contextAdapter.Context.Accounts.AreActive().AreNotExcluded().Select(x => x.CurrentBalance).ToListAsync(cancellationToken)).Sum();
            }
        }
    }

}
