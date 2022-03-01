namespace MoneyFox.Core.Queries.Accounts.GetIncludedAccount
{
    using _Pending_.Common.Interfaces;
    using _Pending_.Common.QueryObjects;
    using Aggregates;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetIncludedAccountQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetIncludedAccountQuery, List<Account>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Account>> Handle(GetIncludedAccountQuery request,
                CancellationToken cancellationToken) =>
                await contextAdapter.Context
                    .Accounts
                    .AreActive()
                    .AreNotExcluded()
                    .OrderByName()
                    .ToListAsync(cancellationToken);
        }
    }
}