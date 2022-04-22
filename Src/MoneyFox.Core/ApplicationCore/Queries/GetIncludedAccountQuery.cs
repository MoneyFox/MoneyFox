namespace MoneyFox.Core.ApplicationCore.Queries
{

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates.AccountAggregate;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using MoneyFox.Core._Pending_.Common.QueryObjects;
    using MoneyFox.Core.Common.Interfaces;

    public class GetIncludedAccountQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetIncludedAccountQuery, List<Account>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Account>> Handle(GetIncludedAccountQuery request, CancellationToken cancellationToken)
            {
                return await contextAdapter.Context.Accounts.AreActive().AreNotExcluded().OrderByName().ToListAsync(cancellationToken);
            }
        }
    }

}
