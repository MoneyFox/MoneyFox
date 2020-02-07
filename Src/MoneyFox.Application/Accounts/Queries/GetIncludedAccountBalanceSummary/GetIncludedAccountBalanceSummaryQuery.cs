using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;

namespace MoneyFox.Application.Accounts.Queries.GetIncludedAccountBalanceSummary
{
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
                var accountsList = await contextAdapter.Context
                                           .Accounts
                                           .AreNotExcluded()
                                           .ToListAsync(cancellationToken);

                return accountsList.Sum(x => x.CurrentBalance);
            }
        }
    }
}
