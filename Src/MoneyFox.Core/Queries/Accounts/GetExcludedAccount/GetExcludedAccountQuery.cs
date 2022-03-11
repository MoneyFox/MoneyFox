namespace MoneyFox.Core.Queries.Accounts.GetExcludedAccount
{
    using _Pending_.Common.QueryObjects;
    using Aggregates;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetExcludedAccountQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetExcludedAccountQuery, List<Account>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Account>> Handle(GetExcludedAccountQuery request,
                CancellationToken cancellationToken) =>
                await contextAdapter.Context
                    .Accounts
                    .AreActive()
                    .AreExcluded()
                    .OrderByName()
                    .ToListAsync(cancellationToken);
        }
    }
}