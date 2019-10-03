using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.Application.QueryObjects;

namespace MoneyFox.Application.Accounts.Queries
{
    public class GetIncludedAccountBalanceSummary : IRequest<double>
    {
        public class Handler : IRequestHandler<GetIncludedAccountBalanceSummary, double>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<double> Handle(GetIncludedAccountBalanceSummary request, CancellationToken cancellationToken)
            {
                return await context.Accounts.AreNotExcluded().SumAsync(x => x.CurrentBalance, cancellationToken);
            }
        }
    }
}
