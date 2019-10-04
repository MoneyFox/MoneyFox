using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.Application.QueryObjects;

namespace MoneyFox.Application.Accounts.Queries.GetIncludedAccountBalanceSummary
{
    public class GetIncludedAccountBalanceSummaryQuery : IRequest<double>
    {
        public class Handler : IRequestHandler<GetIncludedAccountBalanceSummaryQuery, double>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<double> Handle(GetIncludedAccountBalanceSummaryQuery request, CancellationToken cancellationToken)
            {
                return await context.Accounts.AreNotExcluded().SumAsync(x => x.CurrentBalance, cancellationToken);
            }
        }
    }
}
