namespace MoneyFox.Core.Queries.Accounts.GetAccounts
{
    using _Pending_.Common.QueryObjects;
    using Aggregates;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetAccountsQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetAccountsQuery, List<Account>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Account>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
            {
                List<Account>? accounts = await contextAdapter.Context
                    .Accounts
                    .AreActive()
                    .OrderByInclusion()
                    .OrderByName()
                    .ToListAsync(cancellationToken);

                return accounts;
            }
        }
    }
}