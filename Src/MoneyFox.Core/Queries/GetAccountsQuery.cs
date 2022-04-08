namespace MoneyFox.Core.Queries
{

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using _Pending_.Common.QueryObjects;
    using Aggregates;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

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
                var accounts = await contextAdapter.Context.Accounts.AreActive().OrderByInclusion().OrderByName().ToListAsync(cancellationToken);

                return accounts;
            }
        }
    }

}
