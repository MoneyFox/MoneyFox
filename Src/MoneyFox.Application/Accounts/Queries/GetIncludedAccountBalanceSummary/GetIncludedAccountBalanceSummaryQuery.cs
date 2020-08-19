using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                var balance = contextAdapter.Context
                                            .Accounts
                                            .AreNotExcluded()
                                            .Sum(x => x.CurrentBalance);

                return await Task.FromResult(balance);
            }
        }
    }
}
