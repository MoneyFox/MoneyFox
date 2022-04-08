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

    public class GetExcludedAccountQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetExcludedAccountQuery, List<Account>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Account>> Handle(GetExcludedAccountQuery request, CancellationToken cancellationToken)
            {
                return await contextAdapter.Context.Accounts.AreActive().AreExcluded().OrderByName().ToListAsync(cancellationToken);
            }
        }
    }

}
