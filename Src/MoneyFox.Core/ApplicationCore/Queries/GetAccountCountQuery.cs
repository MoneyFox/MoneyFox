namespace MoneyFox.Core.ApplicationCore.Queries
{

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using MoneyFox.Core._Pending_.Common.QueryObjects;
    using MoneyFox.Core.Common.Interfaces;

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
                return await contextAdapter.Context.Accounts.AreActive().CountAsync(cancellationToken);
            }
        }
    }

}
