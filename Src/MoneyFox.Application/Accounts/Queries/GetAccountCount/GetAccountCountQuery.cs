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
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<int> Handle(GetAccountCountQuery request, CancellationToken cancellationToken)
            {
                return await context.Accounts.CountAsync(cancellationToken);
            }
        }
    }
}
