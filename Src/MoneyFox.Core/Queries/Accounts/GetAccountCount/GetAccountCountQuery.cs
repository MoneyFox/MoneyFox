using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.QueryObjects;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Queries.Accounts.GetAccountCount
{
    public class GetAccountCountQuery : IRequest<int>
    {
        public class Handler : IRequestHandler<GetAccountCountQuery, int>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<int> Handle(GetAccountCountQuery request, CancellationToken cancellationToken)
                => await contextAdapter.Context.Accounts.AreActive()
                    .CountAsync(cancellationToken);
        }
    }
}