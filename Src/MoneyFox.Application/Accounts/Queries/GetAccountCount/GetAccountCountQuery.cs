using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;

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
            {
                return await contextAdapter.Context.Accounts.CountAsync(cancellationToken);
            }
        }
    }
}
