namespace MoneyFox.Core.Queries.Accounts.GetAccountCount
{
    using _Pending_.Common.Interfaces;
    using _Pending_.Common.QueryObjects;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

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