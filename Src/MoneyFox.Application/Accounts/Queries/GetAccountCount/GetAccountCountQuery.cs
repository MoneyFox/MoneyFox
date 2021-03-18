using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Accounts.Queries.GetAccountCount
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
