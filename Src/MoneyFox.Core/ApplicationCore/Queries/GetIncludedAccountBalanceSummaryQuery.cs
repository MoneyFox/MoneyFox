namespace MoneyFox.Core.ApplicationCore.Queries;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetIncludedAccountBalanceSummaryQuery : IRequest<decimal>
{
    public class Handler : IRequestHandler<GetIncludedAccountBalanceSummaryQuery, decimal>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<decimal> Handle(GetIncludedAccountBalanceSummaryQuery request, CancellationToken cancellationToken)
        {
            return (await appDbContext.Accounts.AreActive().AreNotExcluded().Select(x => x.CurrentBalance).ToListAsync(cancellationToken)).Sum();
        }
    }
}
